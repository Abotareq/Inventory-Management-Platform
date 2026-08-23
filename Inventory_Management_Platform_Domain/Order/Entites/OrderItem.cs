using Inventory_Management_Platform.Domain.Common.Models;
using Inventory_Management_Platform.Domain.Order.ValueObjects;
using Inventory_Management_Platform.Domain.Product.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Domain.Order.Entites
{
    public sealed class OrderItem : Entity
    {
        public OrderItemId OrderItemId { get; private set; }
        public OrderId OrderId { get; private set; }
        public ProductId ProductId { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitPriceSnapshot { get; private set; }
        public decimal LineTotal => Quantity * UnitPriceSnapshot;

        private OrderItem(
            OrderItemId orderItemId,
            OrderId orderId,
            ProductId productId,
            int quantity,
            decimal unitPriceSnapshot)
            : base(orderItemId.Value)
        {
            OrderItemId = orderItemId;
            OrderId = orderId;
            ProductId = productId;
            Quantity = quantity;
            UnitPriceSnapshot = unitPriceSnapshot;
        }

        private OrderItem() { }

        public static OrderItem Create(
            OrderId orderId, ProductId productId, int quantity, decimal unitPriceSnapshot)
        {
            return new OrderItem(
                OrderItemId.CreateUnique(), orderId, productId, quantity, unitPriceSnapshot);
        }
    }
}
