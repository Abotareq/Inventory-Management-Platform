using ErrorOr;
using Inventory_Management_Platform.Application.Common.Interfaces.Persistence;
using Inventory_Management_Platform.Domain.DomainErrors;
using Inventory_Management_Platform.Domain.Warehouse.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Warehouses.Commands.DeleteWarehouse
{
    public sealed class DeleteWarehouseCommandHandler
         : IRequestHandler<DeleteWarehouseCommand, ErrorOr<Deleted>>
    {
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteWarehouseCommandHandler(
            IWarehouseRepository warehouseRepository,
            IUnitOfWork unitOfWork)
        {
            _warehouseRepository = warehouseRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<Deleted>> Handle(
            DeleteWarehouseCommand request, CancellationToken cancellationToken)
        {
            var warehouseId = WarehouseId.Create(request.WarehouseId);

            var warehouse = await _warehouseRepository.GetByIdAsync(warehouseId);
            if (warehouse is null)
                return Errors.Warehouse.NotFound;

            if (await _warehouseRepository.HasStockAsync(warehouseId))
                return Errors.Warehouse.HasStock;

            _warehouseRepository.Delete(warehouse);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Deleted;
        }
    }
}
