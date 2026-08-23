using ErrorOr;
using Inventory_Management_Platform.Domain.Common.Models;
using Inventory_Management_Platform.Domain.DomainErrors;
using Inventory_Management_Platform.Domain.Order.ValueObjects;
using Inventory_Management_Platform.Domain.Product.ValueObjects;
using Inventory_Management_Platform.Domain.Stock.Events;
using Inventory_Management_Platform.Domain.Stock.ValueObjects;
using Inventory_Management_Platform.Domain.Warehouse.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Domain.Stock
{
    public sealed class Stock : AggregateRoot
    {
        public StockId StockId { get; private set; }
        public ProductId ProductId { get; private set; }
        public WarehouseId WarehouseId { get; private set; }
        public int Quantity { get; private set; }      // physical on-hand
        public int Reserved { get; private set; }       // committed to pending orders, not yet shipped
        public byte[] RowVersion { get; private set; } = Array.Empty<byte>();

        public int Available => Quantity - Reserved;

        private Stock(StockId stockId, ProductId productId, WarehouseId warehouseId, int quantity, int reserved)
            : base(stockId.Value)
        {
            StockId = stockId;
            ProductId = productId;
            WarehouseId = warehouseId;
            Quantity = quantity;
            Reserved = reserved;
        }

        private Stock() { }

        public static ErrorOr<Stock> Create(ProductId productId, WarehouseId warehouseId)
        {
            return new Stock(StockId.CreateUnique(), productId, warehouseId, quantity: 0, reserved: 0);
        }

        public ErrorOr<Updated> Increase(int amount, string reason, Guid performedByUserId)
        {
            if (amount <= 0)
                return Errors.Stock.InvalidAdjustmentAmount;

            if (string.IsNullOrWhiteSpace(reason))
                return Errors.Stock.ReasonIsRequired;

            Quantity += amount;

            RaiseDomainEvent(new StockIncreased(StockId, amount, Quantity, reason, performedByUserId, DateTime.UtcNow));

            return Result.Updated;
        }

        public ErrorOr<Updated> Decrease(int amount, string reason, Guid performedByUserId)
        {
            if (amount <= 0)
                return Errors.Stock.InvalidAdjustmentAmount;

            if (string.IsNullOrWhiteSpace(reason))
                return Errors.Stock.ReasonIsRequired;

            // Physical quantity must never drop below what's already committed to orders
            if (Quantity - amount < Reserved)
                return Errors.Stock.InsufficientStock;

            Quantity -= amount;

            RaiseDomainEvent(new StockDecreased(StockId, amount, Quantity, reason, performedByUserId, DateTime.UtcNow));

            return Result.Updated;
        }

        public ErrorOr<Updated> Reserve(int amount, OrderId orderId, Guid performedByUserId)
        {
            if (amount <= 0)
                return Errors.Stock.InvalidAdjustmentAmount;

            if (Available - amount < 0)
                return Errors.Stock.InsufficientStock;

            Reserved += amount;

            RaiseDomainEvent(new StockReserved(StockId, amount, Reserved, orderId, performedByUserId, DateTime.UtcNow));

            return Result.Updated;
        }

        public ErrorOr<Updated> Release(int amount, OrderId orderId, Guid performedByUserId)
        {
            if (amount <= 0)
                return Errors.Stock.InvalidAdjustmentAmount;

            if (Reserved - amount < 0)
                return Errors.Stock.InvalidReleaseAmount;

            Reserved -= amount;

            RaiseDomainEvent(new StockReleased(StockId, amount, Reserved, orderId, performedByUserId, DateTime.UtcNow));

            return Result.Updated;
        }

        public ErrorOr<Updated> Commit(int amount, OrderId orderId, Guid performedByUserId)
        {
            if (amount <= 0)
                return Errors.Stock.InvalidAdjustmentAmount;

            if (Reserved - amount < 0)
                return Errors.Stock.InvalidReleaseAmount;

            Reserved -= amount;
            Quantity -= amount;

            RaiseDomainEvent(new StockCommitted(StockId, amount, Quantity, orderId, performedByUserId, DateTime.UtcNow));

            return Result.Updated;
        }
    }
}
