using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Contracts.Stock
{
    public sealed record AssignProductToWarehouseRequest(
     Guid ProductId,
     Guid WarehouseId);
}
