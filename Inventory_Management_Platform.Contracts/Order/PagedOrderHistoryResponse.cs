using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Contracts.Order
{
    public sealed record PagedOrderHistoryResponse(
        List<OrderHistoryResponse> Items,
        int PageNumber,
        int PageSize,
        int TotalCount);
}
