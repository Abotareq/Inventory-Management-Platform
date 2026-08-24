using ErrorOr;
using Inventory_Management_Platform.Application.Common.Interfaces.Persistence;
using Inventory_Management_Platform.Domain.DomainErrors;
using Inventory_Management_Platform.Domain.Stock.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Stocks.Commands.DeleteStock
{
    public sealed class DeleteStockCommandHandler
       : IRequestHandler<DeleteStockCommand, ErrorOr<Deleted>>
    {
        private readonly IStockRepository _stockRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteStockCommandHandler(
            IStockRepository stockRepository,
            IUnitOfWork unitOfWork)
        {
            _stockRepository = stockRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<Deleted>> Handle(
            DeleteStockCommand request, CancellationToken cancellationToken)
        {
            var stockId = StockId.Create(request.StockId);

            var stock = await _stockRepository.GetByIdAsync(stockId);
            if (stock is null)
                return Errors.Stock.NotFound;

            if (stock.Reserved > 0)
                return Errors.Stock.HasActiveReservations;

            if (stock.Quantity > 0)
                return Errors.Stock.HasQuantity;

            if (await _stockRepository.HasOrderItemsAsync(stock.ProductId, stock.WarehouseId))
                return Errors.Stock.HasOrderHistory;

            _stockRepository.Delete(stock);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Deleted;
        }
    }
}
