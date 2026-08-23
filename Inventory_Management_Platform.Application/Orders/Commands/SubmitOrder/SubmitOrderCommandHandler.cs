using ErrorOr;
using Inventory_Management_Platform.Application.Common.Exceptions;
using Inventory_Management_Platform.Application.Common.Interfaces.Persistence;
using Inventory_Management_Platform.Contracts.Order;
using Inventory_Management_Platform.Domain.DomainErrors;
using Inventory_Management_Platform.Domain.Order;
using Inventory_Management_Platform.Domain.Order.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Orders.Commands.SubmitOrder
{
    public sealed class SubmitOrderCommandHandler
            : IRequestHandler<SubmitOrderCommand, ErrorOr<OrderResponse>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SubmitOrderCommandHandler(
            IOrderRepository orderRepository,
            IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<OrderResponse>> Handle(
            SubmitOrderCommand request, CancellationToken cancellationToken)
        {
            var orderId = OrderId.Create(request.OrderId);

            var order = await _orderRepository.GetByIdWithItemsAsync(orderId);
            if (order is null)
                return Errors.Order.NotFound;

            var submitResult = order.Submit(request.PerformedByUserId);
            if (submitResult.IsError)
                return submitResult.Errors;

            try
            {
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
            catch (ConcurrencyConflictException)
            {
                return Errors.Order.ConcurrencyConflict;
            }

            return MapToResponse(order);
        }

        private static OrderResponse MapToResponse(Order order)
        {
            var items = order.Items
                .Select(i => new OrderItemResponse(
                    i.OrderItemId.Value, i.ProductId.Value, i.Quantity, i.UnitPriceSnapshot, i.LineTotal))
                .ToList();

            return new OrderResponse(
                order.OrderId.Value, order.CustomerId.Value, order.Status.ToString(),
                order.CreatedAt, order.TotalAmount, items);
        }
    }
}
