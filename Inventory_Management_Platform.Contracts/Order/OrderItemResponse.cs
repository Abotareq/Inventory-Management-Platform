using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Contracts.Order
{
    public sealed record OrderItemResponse(
      Guid OrderItemId,
      Guid ProductId,
      int Quantity,
      decimal UnitPriceSnapshot,
      decimal LineTotal);
}
