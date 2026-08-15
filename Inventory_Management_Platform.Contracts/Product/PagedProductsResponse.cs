using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Contracts.Product
{
    public sealed record PagedProductsResponse(
          List<ProductResponse> Items,
          int PageNumber,
          int PageSize,
          int TotalCount);
}
