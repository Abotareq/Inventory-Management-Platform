using ErrorOr;
using Inventory_Management_Platform.Application.Common.Exceptions;
using Inventory_Management_Platform.Application.Common.Interfaces.Persistence;
using Inventory_Management_Platform.Application.Orders.Common;
using Inventory_Management_Platform.Contracts.Order;
using Inventory_Management_Platform.Domain.DomainErrors;
using Inventory_Management_Platform.Domain.Order;
using Inventory_Management_Platform.Domain.Order.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Orders.Commands.BeginProcessing
{
    public sealed class BeginProcessingCommandHandler
          : IRequestHandler<BeginProcessingCommand, ErrorOr<OrderResponse>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IStockRepository _stockRepository;
        private readonly IUnitOfWork _unitOfWork;

        public BeginProcessingCommandHandler(
            IOrderRepository orderRepository,
            IStockRepository stockRepository,
            IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _stockRepository = stockRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<OrderResponse>> Handle(
            BeginProcessingCommand request, CancellationToken cancellationToken)
        {
            var orderId = OrderId.Create(request.OrderId);

            var order = await _orderRepository.GetByIdWithItemsAsync(orderId);
            if (order is null)
                return Errors.Order.NotFound;

            // Order.BeginProcessing() only flips status; stock availability is checked here
            // because it requires cross-aggregate lookups the Order aggregate can't perform itself.
            var beginResult = order.BeginProcessing(request.PerformedByUserId);
            if (beginResult.IsError)
                return beginResult.Errors;

            // Assumes one Stock record per Product for simplicity — reserving against a specific
            // warehouse would need the order/order item to carry a WarehouseId, which isn't modeled yet.
            foreach (var item in order.Items)
            {
                var stock = await _stockRepository.GetByProductAndWarehouseAsync(item.ProductId, item.WarehouseId);
                if (stock is null)
                    return Errors.Stock.NotFound;

                var reserveResult = stock.Reserve(item.Quantity, orderId, request.PerformedByUserId);
                if (reserveResult.IsError)
                    return reserveResult.Errors;
            }

            try
            {
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
            catch (ConcurrencyConflictException)
            {
                return Errors.Order.ConcurrencyConflict;
            }

            return order.ToResponse();
        }
        
    }
}
