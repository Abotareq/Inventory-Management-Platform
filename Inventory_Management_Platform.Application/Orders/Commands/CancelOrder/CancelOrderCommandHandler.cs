using ErrorOr;
using Inventory_Management_Platform.Application.Common.Exceptions;
using Inventory_Management_Platform.Application.Common.Interfaces.Persistence;
using Inventory_Management_Platform.Application.Orders.Common;
using Inventory_Management_Platform.Contracts.Order;
using Inventory_Management_Platform.Domain.DomainErrors;
using Inventory_Management_Platform.Domain.Order.Enums;
using Inventory_Management_Platform.Domain.Order.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Orders.Commands.CancelOrder
{
    public sealed class CancelOrderCommandHandler
        : IRequestHandler<CancelOrderCommand, ErrorOr<OrderResponse>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IStockRepository _stockRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CancelOrderCommandHandler> _logger;
        public CancelOrderCommandHandler(
            IOrderRepository orderRepository,
            IStockRepository stockRepository,
            IUnitOfWork unitOfWork,
            ILogger<CancelOrderCommandHandler> logger)
        {
            _orderRepository = orderRepository;
            _stockRepository = stockRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ErrorOr<OrderResponse>> Handle(
            CancelOrderCommand request, CancellationToken cancellationToken)
        {
            var orderId = OrderId.Create(request.OrderId);

            var order = await _orderRepository.GetByIdWithItemsAsync(orderId);
            if (order is null)
                return Errors.Order.NotFound;

            var statusBeforeCancel = order.Status;

            var cancelResult = order.Cancel(request.PerformedByUserId);
            if (cancelResult.IsError)
                return cancelResult.Errors;

            // Stock is only ever reserved once BeginProcessing runs, not at Submit.
            var stockWasReserved = statusBeforeCancel is OrderStatus.Processing;

            if (stockWasReserved)
            {
                foreach (var item in order.Items)
                {
                    var stock = await _stockRepository.GetByProductAndWarehouseAsync(
                        item.ProductId, item.WarehouseId);

                    if (stock is null)
                        return Errors.Stock.NotFound;

                    var releaseResult = stock.Release(item.Quantity, orderId, request.PerformedByUserId);
                    if (releaseResult.IsError)
                        return releaseResult.Errors;
                }
            }

            try
            {
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
            catch (ConcurrencyConflictException)
            {
                _logger.LogWarning("Concurrency conflict cancelling order. OrderId: {OrderId}", request.OrderId);

                return Errors.Order.ConcurrencyConflict;
            }

            return order.ToResponse();
        }
    }
}
