using ErrorOr;
using Inventory_Management_Platform.Application.Common.Interfaces.Persistence;
using Inventory_Management_Platform.Contracts.Stock;
using Inventory_Management_Platform.Domain.DomainErrors;
using Inventory_Management_Platform.Domain.Stock.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Stocks.Queries.GetStockReservationHistory
{
    public sealed class GetStockReservationHistoryQueryHandler
       : IRequestHandler<GetStockReservationHistoryQuery, ErrorOr<PagedStockReservationsResponse>>
    {
        private readonly IStockRepository _stockRepository;

        public GetStockReservationHistoryQueryHandler(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }

        public async Task<ErrorOr<PagedStockReservationsResponse>> Handle(
            GetStockReservationHistoryQuery request, CancellationToken cancellationToken)
        {
            var stockId = StockId.Create(request.StockId);

            var stock = await _stockRepository.GetByIdAsync(stockId);
            if (stock is null)
                return Errors.Stock.NotFound;

            var (items, totalCount) = await _stockRepository.GetReservationHistoryAsync(
                stockId, request.PageNumber, request.PageSize);

            var response = items
                .Select(r => new StockReservationResponse(
                    r.StockReservationId.Value, r.StockId.Value, r.OrderId.Value,
                    r.Amount, r.Action, r.PerformedByUserId, r.Timestamp))
                .ToList();

            return new PagedStockReservationsResponse(response, request.PageNumber, request.PageSize, totalCount);
        }
    }
}
