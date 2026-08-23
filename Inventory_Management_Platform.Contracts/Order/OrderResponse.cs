using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Contracts.Order
{
    public sealed record OrderResponse(
    Guid OrderId,
    Guid CustomerId,
    string Status,
    DateTime CreatedAt,
    decimal TotalAmount,
    List<OrderItemResponse> Items);
}
