using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Stocks.Commands.DeleteStock
{
    public sealed record DeleteStockCommand(Guid StockId) : IRequest<ErrorOr<Deleted>>;

}
