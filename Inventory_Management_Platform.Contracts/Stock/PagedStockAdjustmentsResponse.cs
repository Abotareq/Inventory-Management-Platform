using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Contracts.Stock
{
    public sealed record PagedStockAdjustmentsResponse(
       List<StockAdjustmentResponse> Items,
       int PageNumber,
       int PageSize,
       int TotalCount);
}
