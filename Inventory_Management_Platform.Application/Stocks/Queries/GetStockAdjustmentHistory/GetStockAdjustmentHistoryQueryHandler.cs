using ErrorOr;
using Inventory_Management_Platform.Application.Common.Interfaces.Persistence;
using Inventory_Management_Platform.Contracts.Stock;
using Inventory_Management_Platform.Domain.DomainErrors;
using Inventory_Management_Platform.Domain.Stock.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Stocks.Queries.GetStockAdjustmentHistory
{
    public sealed class GetStockAdjustmentHistoryQueryHandler
        : IRequestHandler<GetStockAdjustmentHistoryQuery, ErrorOr<PagedStockAdjustmentsResponse>>
    {
        private readonly IStockRepository _stockRepository;

        public GetStockAdjustmentHistoryQueryHandler(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }

        public async Task<ErrorOr<PagedStockAdjustmentsResponse>> Handle(
            GetStockAdjustmentHistoryQuery request, CancellationToken cancellationToken)
        {
            var stockId = StockId.Create(request.StockId);

            var stock = await _stockRepository.GetByIdAsync(stockId);
            if (stock is null)
                return Errors.Stock.NotFound;

            var (items, totalCount) = await _stockRepository.GetAdjustmentHistoryAsync(
                stockId, request.PageNumber, request.PageSize);

            var response = items
                .Select(a => new StockAdjustmentResponse(
                    a.StockAdjustmentId.Value,
                    a.StockId.Value,
                    a.Delta,
                    a.ResultingQuantity,
                    a.Reason,
                    a.PerformedByUserId,
                    a.Timestamp))
                .ToList();

            return new PagedStockAdjustmentsResponse(
                response, request.PageNumber, request.PageSize, totalCount);
        }
    }
}
