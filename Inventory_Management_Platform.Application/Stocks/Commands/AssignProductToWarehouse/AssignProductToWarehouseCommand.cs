using ErrorOr;
using Inventory_Management_Platform.Contracts.Stock;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Stocks.Commands.AssignProductToWarehouse
{
    public sealed record AssignProductToWarehouseCommand(
            Guid ProductId,
            Guid WarehouseId) : IRequest<ErrorOr<StockResponse>>;
}
