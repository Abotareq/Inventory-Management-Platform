using ErrorOr;
using Inventory_Management_Platform.Contracts.Stock;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Stocks.Queries.GetStockAdjustmentHistory
{
    public sealed record GetStockAdjustmentHistoryQuery(
       Guid StockId,
       int PageNumber,
       int PageSize) : IRequest<ErrorOr<PagedStockAdjustmentsResponse>>;
}
