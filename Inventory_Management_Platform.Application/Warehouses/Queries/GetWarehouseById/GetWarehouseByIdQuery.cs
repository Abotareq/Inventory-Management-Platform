using ErrorOr;
using Inventory_Management_Platform.Contracts.Warehouse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Warehouses.Queries.GetWarehouseById
{
    public sealed record GetWarehouseByIdQuery(Guid WarehouseId) : IRequest<ErrorOr<WarehouseResponse>>;

}
