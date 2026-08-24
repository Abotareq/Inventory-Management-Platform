using ErrorOr;
using Inventory_Management_Platform.Application.Common.Interfaces.Persistence;
using Inventory_Management_Platform.Contracts.Stock;
using Inventory_Management_Platform.Domain.Product.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Stocks.Queries.GetStockByProduct
{
    public sealed class GetStockByProductQueryHandler
        : IRequestHandler<GetStockByProductQuery, ErrorOr<PagedStockResponse>>
    {
        private readonly IStockRepository _stockRepository;

        public GetStockByProductQueryHandler(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }

        public async Task<ErrorOr<PagedStockResponse>> Handle(
            GetStockByProductQuery request, CancellationToken cancellationToken)
        {
            var productId = ProductId.Create(request.ProductId);

            var (items, totalCount) = await _stockRepository.GetByProductAsync(
                productId, request.PageNumber, request.PageSize);

            var response = items
                .Select(stock => new StockResponse(
    stock.StockId.Value, stock.ProductId.Value, stock.WarehouseId.Value,
    stock.Quantity, stock.Reserved, stock.Available))
                .ToList();

            return new PagedStockResponse(response, request.PageNumber, request.PageSize, totalCount);
        }
    }
}
