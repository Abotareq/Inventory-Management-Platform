using ErrorOr;
using Inventory_Management_Platform.Contracts.Warehouse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Warehouses.Commands.CreateWarehouse
{
    public sealed record CreateWarehouseCommand(
         string Name,
         string Location) : IRequest<ErrorOr<WarehouseResponse>>;
}
