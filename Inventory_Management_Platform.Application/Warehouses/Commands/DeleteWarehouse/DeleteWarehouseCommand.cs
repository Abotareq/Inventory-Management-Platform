using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Warehouses.Commands.DeleteWarehouse
{
    public sealed record DeleteWarehouseCommand(Guid WarehouseId) : IRequest<ErrorOr<Deleted>>;

}
