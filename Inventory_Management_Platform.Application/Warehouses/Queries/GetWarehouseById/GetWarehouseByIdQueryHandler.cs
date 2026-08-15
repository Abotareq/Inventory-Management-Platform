using ErrorOr;
using Inventory_Management_Platform.Application.Common.Interfaces.Persistence;
using Inventory_Management_Platform.Contracts.Warehouse;
using Inventory_Management_Platform.Domain.DomainErrors;
using Inventory_Management_Platform.Domain.Warehouse.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Warehouses.Queries.GetWarehouseById
{
    public sealed class GetWarehouseByIdQueryHandler
          : IRequestHandler<GetWarehouseByIdQuery, ErrorOr<WarehouseResponse>>
    {
        private readonly IWarehouseRepository _warehouseRepository;

        public GetWarehouseByIdQueryHandler(IWarehouseRepository warehouseRepository)
        {
            _warehouseRepository = warehouseRepository;
        }

        public async Task<ErrorOr<WarehouseResponse>> Handle(
            GetWarehouseByIdQuery request, CancellationToken cancellationToken)
        {
            var warehouse = await _warehouseRepository.GetByIdAsync(
                WarehouseId.Create(request.WarehouseId));

            if (warehouse is null)
                return Errors.Warehouse.NotFound;

            return new WarehouseResponse(
                warehouse.WarehouseId.Value, warehouse.Name, warehouse.Location);
        }
    }
}
