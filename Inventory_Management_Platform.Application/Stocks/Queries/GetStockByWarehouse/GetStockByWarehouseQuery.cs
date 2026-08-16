using ErrorOr;
using Inventory_Management_Platform.Contracts.Stock;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Stocks.Queries.GetStockByWarehouse
{
    public sealed record GetStockByWarehouseQuery(
       Guid WarehouseId,
       int PageNumber,
       int PageSize) : IRequest<ErrorOr<PagedStockResponse>>;
}
