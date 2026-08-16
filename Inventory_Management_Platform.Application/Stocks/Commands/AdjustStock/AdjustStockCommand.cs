using ErrorOr;
using Inventory_Management_Platform.Contracts.Stock;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Stocks.Commands.AdjustStock
{
    public sealed record AdjustStockCommand(
         Guid ProductId,
         Guid WarehouseId,
         int Amount,
         string Reason,
         Guid PerformedByUserId) : IRequest<ErrorOr<StockResponse>>;
}
