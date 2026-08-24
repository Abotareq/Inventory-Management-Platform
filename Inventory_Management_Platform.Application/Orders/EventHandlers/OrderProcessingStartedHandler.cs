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
    public sealed class OrderProcessingStartedHandler : INotificationHandler<OrderProcessingStarted>
    {
        private readonly IOrderRepository _orderRepository;

        public OrderProcessingStartedHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task Handle(OrderProcessingStarted notification, CancellationToken cancellationToken)
        {
            var history = OrderHistory.Create(
                notification.OrderId, notification.FromStatus, OrderStatus.Processing,
                notification.PerformedByUserId, notification.Timestamp);

            await _orderRepository.AddHistoryAsync(history);
        }
    }
}
