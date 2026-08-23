using ErrorOr;
using Inventory_Management_Platform.Application.Common.Interfaces.Persistence;
using Inventory_Management_Platform.Contracts.Product;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Products.Queries.GetProducts
{
    public sealed class GetProductsQueryHandler
        : IRequestHandler<GetProductsQuery, ErrorOr<PagedProductsResponse>>
    {
        private readonly IProductRepository _productRepository;

        public GetProductsQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ErrorOr<PagedProductsResponse>> Handle(
            GetProductsQuery request, CancellationToken cancellationToken)
        {
            var (items, totalCount) = await _productRepository.GetPagedAsync(
                request.PageNumber, request.PageSize);

            var response = items
                .Select(p => new ProductResponse(
                    p.ProductId.Value, p.Name, p.Sku, p.Description, p.CategoryId?.Value, p.Price))
                .ToList();

            return new PagedProductsResponse(
                response, request.PageNumber, request.PageSize, totalCount);
        }
    }
}
