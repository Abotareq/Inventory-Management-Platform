using Inventory_Management_Platform.Domain.Common.Models;
using Inventory_Management_Platform.Domain.Order.ValueObjects;
using Inventory_Management_Platform.Domain.Stock.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Domain.Stock.Entites
{
    public sealed class StockReservation : Entity
    {
        public StockReservationId StockReservationId { get; private set; }
        public StockId StockId { get; private set; }
        public OrderId OrderId { get; private set; }
        public int Amount { get; private set; }
        public string Action { get; private set; } // "Reserved" | "Released" | "Committed"
        public Guid PerformedByUserId { get; private set; }
        public DateTime Timestamp { get; private set; }

        private StockReservation(
            StockReservationId stockReservationId,
            StockId stockId,
            OrderId orderId,
            int amount,
            string action,
            Guid performedByUserId,
            DateTime timestamp)
            : base(stockReservationId.Value)
        {
            StockReservationId = stockReservationId;
            StockId = stockId;
            OrderId = orderId;
            Amount = amount;
            Action = action;
            PerformedByUserId = performedByUserId;
            Timestamp = timestamp;
        }

        private StockReservation() { }

        public static StockReservation Create(
            StockId stockId, OrderId orderId, int amount, string action,
            Guid performedByUserId, DateTime timestamp)
        {
            return new StockReservation(
                StockReservationId.CreateUnique(), stockId, orderId, amount, action, performedByUserId, timestamp);
        }
    }
}
