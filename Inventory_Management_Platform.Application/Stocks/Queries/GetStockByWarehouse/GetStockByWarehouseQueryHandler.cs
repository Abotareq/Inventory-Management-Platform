using ErrorOr;
using Inventory_Management_Platform.Application.Common.Interfaces.Persistence;
using Inventory_Management_Platform.Contracts.Stock;
using Inventory_Management_Platform.Domain.Warehouse.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Stocks.Queries.GetStockByWarehouse
{
    public sealed class GetStockByWarehouseQueryHandler
      : IRequestHandler<GetStockByWarehouseQuery, ErrorOr<PagedStockResponse>>
    {
        private readonly IStockRepository _stockRepository;

        public GetStockByWarehouseQueryHandler(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }

        public async Task<ErrorOr<PagedStockResponse>> Handle(
            GetStockByWarehouseQuery request, CancellationToken cancellationToken)
        {
            var warehouseId = WarehouseId.Create(request.WarehouseId);

            var (items, totalCount) = await _stockRepository.GetByWarehouseAsync(
                warehouseId, request.PageNumber, request.PageSize);

            var response = items
                .Select(stock => new StockResponse(
    stock.StockId.Value, stock.ProductId.Value, stock.WarehouseId.Value,
    stock.Quantity, stock.Reserved, stock.Available))
                .ToList();

            return new PagedStockResponse(response, request.PageNumber, request.PageSize, totalCount);
        }
    }
}
