using ErrorOr;
using Inventory_Management_Platform.Contracts.Stock;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Stocks.Queries.GetStockByProduct
{
    public sealed record GetStockByProductQuery(
        Guid ProductId,
        int PageNumber,
        int PageSize) : IRequest<ErrorOr<PagedStockResponse>>;
}

