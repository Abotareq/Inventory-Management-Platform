using ErrorOr;
using Inventory_Management_Platform.Application.Common.Interfaces.Persistence;
using Inventory_Management_Platform.Domain.DomainErrors;
using Inventory_Management_Platform.Domain.Product.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Products.Commands.DeleteProduct
{
    public sealed class DeleteProductCommandHandler
        : IRequestHandler<DeleteProductCommand, ErrorOr<Deleted>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteProductCommandHandler(
            IProductRepository productRepository,
            IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<Deleted>> Handle(
            DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var productId = ProductId.Create(request.ProductId);

            var product = await _productRepository.GetByIdAsync(productId);
            if (product is null)
                return Errors.Product.NotFound;

            if (await _productRepository.HasStockAsync(productId))
                return Errors.Product.HasStock;

            _productRepository.Delete(product);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Deleted;
        }
    }
}
