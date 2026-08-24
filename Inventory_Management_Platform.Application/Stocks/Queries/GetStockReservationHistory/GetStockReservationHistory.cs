using ErrorOr;
using Inventory_Management_Platform.Contracts.Stock;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Stocks.Queries.GetStockReservationHistory
{
    public sealed record GetStockReservationHistoryQuery(
            Guid StockId, int PageNumber, int PageSize) : IRequest<ErrorOr<PagedStockReservationsResponse>>;
}
