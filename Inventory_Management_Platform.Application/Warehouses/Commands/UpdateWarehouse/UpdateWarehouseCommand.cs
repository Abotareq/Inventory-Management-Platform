using ErrorOr;
using Inventory_Management_Platform.Contracts.Warehouse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Warehouses.Commands.UpdateWarehouse
{
    public sealed record UpdateWarehouseCommand(
         Guid WarehouseId,
         string Name,
         string Location) : IRequest<ErrorOr<WarehouseResponse>>;
}
