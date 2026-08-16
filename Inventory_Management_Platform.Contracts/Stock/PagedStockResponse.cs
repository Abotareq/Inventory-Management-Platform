using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Contracts.Stock
{
    public sealed record PagedStockResponse(
          List<StockResponse> Items,
          int PageNumber,
          int PageSize,
          int TotalCount);
}
