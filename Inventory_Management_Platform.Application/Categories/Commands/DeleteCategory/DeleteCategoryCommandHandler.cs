using ErrorOr;
using Inventory_Management_Platform.Application.Common.Interfaces.Persistence;
using Inventory_Management_Platform.Domain.Category.ValueObjects;
using Inventory_Management_Platform.Domain.DomainErrors;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Categories.Commands.DeleteCategory
{
    public sealed class DeleteCategoryCommandHandler
         : IRequestHandler<DeleteCategoryCommand, ErrorOr<Deleted>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCategoryCommandHandler(
            ICategoryRepository categoryRepository,
            IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<Deleted>> Handle(
            DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var categoryId = CategoryId.Create(request.CategoryId);

            var category = await _categoryRepository.GetByIdAsync(categoryId);
            if (category is null)
                return Errors.Category.NotFound;

            if (await _categoryRepository.HasProductsAsync(categoryId))
                return Errors.Category.HasProducts;

            _categoryRepository.Delete(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Deleted;
        }
    }
}
