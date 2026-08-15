using ErrorOr;
using Inventory_Management_Platform.Application.Common.Interfaces.Persistence;
using Inventory_Management_Platform.Contracts.Warehouse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Warehouses.Queries.GetWarehouses
{
    public sealed class GetWarehousesQueryHandler
        : IRequestHandler<GetWarehousesQuery, ErrorOr<List<WarehouseResponse>>>
    {
        private readonly IWarehouseRepository _warehouseRepository;

        public GetWarehousesQueryHandler(IWarehouseRepository warehouseRepository)
        {
            _warehouseRepository = warehouseRepository;
        }

        public async Task<ErrorOr<List<WarehouseResponse>>> Handle(
            GetWarehousesQuery request, CancellationToken cancellationToken)
        {
            var warehouses = await _warehouseRepository.GetAllAsync();

            var response = warehouses
                .Select(w => new WarehouseResponse(w.WarehouseId.Value, w.Name, w.Location))
                .ToList();

            return response;
        }
    }
}
