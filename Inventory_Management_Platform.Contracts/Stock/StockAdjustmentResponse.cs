using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Contracts.Stock
{
    public sealed record StockAdjustmentResponse(
         Guid StockAdjustmentId,
         Guid StockId,
         int Delta,
         int ResultingQuantity,
         string Reason,
         Guid PerformedByUserId,
         DateTime Timestamp);
}
