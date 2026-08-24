using Inventory_Management_Platform.Application.Common.Interfaces.Persistence;
using Inventory_Management_Platform.Domain.Order.Entites;
using Inventory_Management_Platform.Domain.Order.Enums;
using Inventory_Management_Platform.Domain.Order.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Orders.EventHandlers
{
    public sealed class OrderSubmittedHandler : INotificationHandler<OrderSubmitted>
    {
        private readonly IOrderRepository _orderRepository;

        public OrderSubmittedHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task Handle(OrderSubmitted notification, CancellationToken cancellationToken)
        {
            var history = OrderHistory.Create(
                notification.OrderId, notification.FromStatus, OrderStatus.Submitted,
                notification.PerformedByUserId, notification.Timestamp);

            await _orderRepository.AddHistoryAsync(history);
        }
    }

}
