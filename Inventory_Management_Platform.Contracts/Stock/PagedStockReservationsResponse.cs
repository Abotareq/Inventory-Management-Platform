using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Contracts.Stock
{

    public sealed record PagedStockReservationsResponse(
        List<StockReservationResponse> Items,
        int PageNumber,
        int PageSize,
        int TotalCount);
}
