using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Contracts.Order
{
    public sealed record PagedOrdersResponse(
        List<OrderResponse> Items,
        int PageNumber,
        int PageSize,
        int TotalCount);
}
