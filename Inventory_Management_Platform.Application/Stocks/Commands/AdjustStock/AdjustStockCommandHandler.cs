using ErrorOr;
using Inventory_Management_Platform.Application.Common.Exceptions;
using Inventory_Management_Platform.Application.Common.Interfaces.Persistence;
using Inventory_Management_Platform.Contracts.Stock;
using Inventory_Management_Platform.Domain.DomainErrors;
using Inventory_Management_Platform.Domain.Product.ValueObjects;
using Inventory_Management_Platform.Domain.Warehouse.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Stocks.Commands.AdjustStock
{
    public sealed class AdjustStockCommandHandler
        : IRequestHandler<AdjustStockCommand, ErrorOr<StockResponse>>
    {
        private readonly IStockRepository _stockRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AdjustStockCommandHandler(
            IStockRepository stockRepository,
            IUnitOfWork unitOfWork)
        {
            _stockRepository = stockRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<StockResponse>> Handle(
    AdjustStockCommand request, CancellationToken cancellationToken)
        {
            var productId = ProductId.Create(request.ProductId);
            var warehouseId = WarehouseId.Create(request.WarehouseId);

            var stock = await _stockRepository.GetByProductAndWarehouseAsync(productId, warehouseId);
            if (stock is null)
                return Errors.Stock.NotFound;

            var adjustResult = request.Amount > 0
                ? stock.Increase(request.Amount, request.Reason, request.PerformedByUserId)
                : stock.Decrease(-request.Amount, request.Reason, request.PerformedByUserId);

            if (adjustResult.IsError)
                return adjustResult.Errors;

            try
            {
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
            catch (ConcurrencyConflictException)
            {
                return Errors.Stock.ConcurrencyConflict;
            }

            return new StockResponse(
    stock.StockId.Value, stock.ProductId.Value, stock.WarehouseId.Value,
    stock.Quantity, stock.Reserved, stock.Available);
        }
    }
}
