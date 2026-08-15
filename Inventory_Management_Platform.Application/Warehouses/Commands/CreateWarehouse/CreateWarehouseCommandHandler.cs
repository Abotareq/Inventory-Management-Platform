using ErrorOr;
using Inventory_Management_Platform.Application.Common.Interfaces.Persistence;
using Inventory_Management_Platform.Contracts.Warehouse;
using Inventory_Management_Platform.Domain.Warehouse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Warehouses.Commands.CreateWarehouse
{
    public sealed class CreateWarehouseCommandHandler
         : IRequestHandler<CreateWarehouseCommand, ErrorOr<WarehouseResponse>>
    {
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateWarehouseCommandHandler(
            IWarehouseRepository warehouseRepository,
            IUnitOfWork unitOfWork)
        {
            _warehouseRepository = warehouseRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<WarehouseResponse>> Handle(
            CreateWarehouseCommand request, CancellationToken cancellationToken)
        {
            var warehouseResult = Warehouse.Create(request.Name, request.Location);
            if (warehouseResult.IsError)
                return warehouseResult.Errors;

            var warehouse = warehouseResult.Value;

            await _warehouseRepository.AddAsync(warehouse);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new WarehouseResponse(
                warehouse.WarehouseId.Value, warehouse.Name, warehouse.Location);
        }
    }
}
