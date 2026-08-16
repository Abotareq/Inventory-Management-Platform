using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Contracts.Stock
{
    public sealed record AdjustStockRequest(
     Guid ProductId,
     Guid WarehouseId,
     int Amount,
     string Reason);
}
