using ErrorOr;
using Inventory_Management_Platform.Contracts.Warehouse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Warehouses.Queries.GetWarehouses
{
    public sealed record GetWarehousesQuery() : IRequest<ErrorOr<List<WarehouseResponse>>>;

}
