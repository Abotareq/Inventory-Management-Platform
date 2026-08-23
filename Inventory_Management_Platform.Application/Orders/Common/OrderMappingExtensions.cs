using Inventory_Management_Platform.Contracts.Order;
using Inventory_Management_Platform.Domain.Order;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Orders.Common
{
    public static class OrderMappingExtensions
    {
        public static OrderResponse ToResponse(this Order order)
        {
            var items = order.Items
                .Select(i => new OrderItemResponse(
                    i.OrderItemId.Value, i.ProductId.Value, i.WarehouseId.Value,
                    i.Quantity, i.UnitPriceSnapshot, i.LineTotal))
                .ToList();

            return new OrderResponse(
                order.OrderId.Value, order.CustomerId.Value, order.Status.ToString(),
                order.CreatedAt, order.TotalAmount, items);
        }
    }
}
