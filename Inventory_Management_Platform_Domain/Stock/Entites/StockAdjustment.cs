using Inventory_Management_Platform.Domain.Common.Models;
using Inventory_Management_Platform.Domain.Stock.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Domain.Stock.Entites
{
    public sealed class StockAdjustment : Entity
    {
        public StockAdjustmentId StockAdjustmentId { get; private set; }
        public StockId StockId { get; private set; }
        public int Delta { get; private set; }
        public int ResultingQuantity { get; private set; }
        public string Reason { get; private set; }
        public Guid PerformedByUserId { get; private set; }
        public DateTime Timestamp { get; private set; }

        private StockAdjustment(
            StockAdjustmentId stockAdjustmentId,
            StockId stockId,
            int delta,
            int resultingQuantity,
            string reason,
            Guid performedByUserId,
            DateTime timestamp)
            : base(stockAdjustmentId.Value)
        {
            StockAdjustmentId = stockAdjustmentId;
            StockId = stockId;
            Delta = delta;
            ResultingQuantity = resultingQuantity;
            Reason = reason;
            PerformedByUserId = performedByUserId;
            Timestamp = timestamp;
        }

        private StockAdjustment() { }

        public static StockAdjustment Create(
            StockId stockId,
            int delta,
            int resultingQuantity,
            string reason,
            Guid performedByUserId,
            DateTime timestamp)
        {
            return new StockAdjustment(
                StockAdjustmentId.CreateUnique(),
                stockId, delta, resultingQuantity, reason, performedByUserId, timestamp);
        }
    }
}
