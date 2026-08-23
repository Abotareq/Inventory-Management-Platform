using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Contracts.Order
{
    public sealed record CreateOrderRequest(
     Guid CustomerId,
     List<OrderItemRequest> Items);
}
