using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Contracts.Order
{
    public sealed record OrderItemRequest(
            Guid ProductId,
                    Guid WarehouseId,

            int Quantity);
}
