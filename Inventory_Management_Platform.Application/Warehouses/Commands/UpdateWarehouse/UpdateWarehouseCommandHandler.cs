using ErrorOr;
using Inventory_Management_Platform.Application.Common.Interfaces.Persistence;
using Inventory_Management_Platform.Contracts.Warehouse;
using Inventory_Management_Platform.Domain.DomainErrors;
using Inventory_Management_Platform.Domain.Warehouse.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Warehouses.Commands.UpdateWarehouse
{
    public sealed class UpdateWarehouseCommandHandler
          : IRequestHandler<UpdateWarehouseCommand, ErrorOr<WarehouseResponse>>
    {
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateWarehouseCommandHandler(
            IWarehouseRepository warehouseRepository,
            IUnitOfWork unitOfWork)
        {
            _warehouseRepository = warehouseRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<WarehouseResponse>> Handle(
            UpdateWarehouseCommand request, CancellationToken cancellationToken)
        {
            var warehouse = await _warehouseRepository.GetByIdAsync(
                WarehouseId.Create(request.WarehouseId));

            if (warehouse is null)
                return Errors.Warehouse.NotFound;

            var updateResult = warehouse.UpdateDetails(request.Name, request.Location);
            if (updateResult.IsError)
                return updateResult.Errors;

            _warehouseRepository.Update(warehouse);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new WarehouseResponse(
                warehouse.WarehouseId.Value, warehouse.Name, warehouse.Location);
        }
    }
}
