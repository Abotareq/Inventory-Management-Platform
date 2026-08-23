using ErrorOr;
using Inventory_Management_Platform.Application.Common.Interfaces.Persistence;
using Inventory_Management_Platform.Contracts.Product;
using Inventory_Management_Platform.Domain.Category.ValueObjects;
using Inventory_Management_Platform.Domain.DomainErrors;
using Inventory_Management_Platform.Domain.Product.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Products.Commands.UpdateProduct
{
    public sealed class UpdateProductCommandHandler
        : IRequestHandler<UpdateProductCommand, ErrorOr<ProductResponse>>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProductCommandHandler(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<ProductResponse>> Handle(
            UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(
                ProductId.Create(request.ProductId));

            if (product is null)
                return Errors.Product.NotFound;

            // Only re-check SKU uniqueness if it actually changed
            if (!string.Equals(product.Sku, request.Sku, StringComparison.Ordinal)
                && await _productRepository.ExistsBySkuAsync(request.Sku))
            {
                return Errors.Product.SkuAlreadyExists;
            }

            CategoryId? categoryId = null;

            if (request.CategoryId is not null)
            {
                categoryId = CategoryId.Create(request.CategoryId.Value);

                var categoryExists = await _categoryRepository.ExistsAsync(categoryId);
                if (!categoryExists)
                    return Errors.Category.NotFound;
            }

            var updateResult = product.UpdateDetails(
                request.Name, request.Sku, request.Description, categoryId, request.Price);

            if (updateResult.IsError)
                return updateResult.Errors;

            _productRepository.Update(product);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new ProductResponse(
                product.ProductId.Value,
                product.Name,
                product.Sku,
                product.Description,
                product.CategoryId?.Value,
                product.Price);
        }
    }
}
