using ErrorOr;
using Inventory_Management_Platform.Application.Common.Interfaces.Persistence;
using Inventory_Management_Platform.Contracts.Product;
using Inventory_Management_Platform.Domain.DomainErrors;
using Inventory_Management_Platform.Domain.Product.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Products.Queries.GetProductById
{
    public sealed class GetProductByIdQueryHandler
        : IRequestHandler<GetProductByIdQuery, ErrorOr<ProductResponse>>
    {
        private readonly IProductRepository _productRepository;

        public GetProductByIdQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ErrorOr<ProductResponse>> Handle(
            GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(
                ProductId.Create(request.ProductId));

            if (product is null)
                return Errors.Product.NotFound;

            return new ProductResponse(
                product.ProductId.Value,
                product.Name,
                product.Sku,
                product.Description,
                product.CategoryId?.Value);
        }
    }
}
