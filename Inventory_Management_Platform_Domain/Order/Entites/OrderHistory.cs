using Inventory_Management_Platform.Domain.Common.Models;
using Inventory_Management_Platform.Domain.Order.Enums;
using Inventory_Management_Platform.Domain.Order.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Domain.Order.Entites
{
    public sealed class OrderHistory : Entity
    {
        public OrderHistoryId OrderHistoryId { get; private set; }
        public OrderId OrderId { get; private set; }
        public OrderStatus FromStatus { get; private set; }
        public OrderStatus ToStatus { get; private set; }
        public Guid PerformedByUserId { get; private set; }
        public DateTime Timestamp { get; private set; }

        private OrderHistory(
            OrderHistoryId orderHistoryId,
            OrderId orderId,
            OrderStatus fromStatus,
            OrderStatus toStatus,
            Guid performedByUserId,
            DateTime timestamp)
            : base(orderHistoryId.Value)
        {
            OrderHistoryId = orderHistoryId;
            OrderId = orderId;
            FromStatus = fromStatus;
            ToStatus = toStatus;
            PerformedByUserId = performedByUserId;
            Timestamp = timestamp;
        }

        private OrderHistory() { }

        public static OrderHistory Create(
            OrderId orderId, OrderStatus fromStatus, OrderStatus toStatus,
            Guid performedByUserId, DateTime timestamp)
        {
            return new OrderHistory(
                OrderHistoryId.CreateUnique(), orderId, fromStatus, toStatus, performedByUserId, timestamp);
        }
    }
}
