using ErrorOr;
using Inventory_Management_Platform.Application.Common.Exceptions;
using Inventory_Management_Platform.Application.Common.Interfaces.Persistence;
using Inventory_Management_Platform.Contracts.Product;
using Inventory_Management_Platform.Domain.Category.ValueObjects;
using Inventory_Management_Platform.Domain.DomainErrors;
using Inventory_Management_Platform.Domain.Product;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Products.Commands.CreateProduct
{
    public sealed class CreateProductCommandHandler
        : IRequestHandler<CreateProductCommand, ErrorOr<ProductResponse>>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateProductCommandHandler(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<ProductResponse>> Handle(
     CreateProductCommand request, CancellationToken cancellationToken)
        {
            if (await _productRepository.ExistsBySkuAsync(request.Sku))
                return Errors.Product.SkuAlreadyExists;

            CategoryId? categoryId = null;

            if (request.CategoryId is not null)
            {
                categoryId = CategoryId.Create(request.CategoryId.Value);

                var categoryExists = await _categoryRepository.ExistsAsync(categoryId);
                if (!categoryExists)
                    return Errors.Category.NotFound;
            }

            var productResult = Product.Create(request.Name, request.Sku, request.Description, categoryId);
            if (productResult.IsError)
                return productResult.Errors;

            var product = productResult.Value;

            try
            {
                await _productRepository.AddAsync(product);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
            catch (UniqueConstraintViolationException)
            {
                return Errors.Product.SkuAlreadyExists;
            }

            return new ProductResponse(product.ProductId.Value, product.Name, product.Sku, product.Description, product.CategoryId?.Value);
        }
    }
}
