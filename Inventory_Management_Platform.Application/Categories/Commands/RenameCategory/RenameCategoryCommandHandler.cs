using ErrorOr;
using Inventory_Management_Platform.Application.Common.Interfaces.Persistence;
using Inventory_Management_Platform.Contracts.Category;
using Inventory_Management_Platform.Domain.Category.ValueObjects;
using Inventory_Management_Platform.Domain.DomainErrors;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Categories.Commands.RenameCategory
{
    public sealed class RenameCategoryCommandHandler
            : IRequestHandler<RenameCategoryCommand, ErrorOr<CategoryResponse>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RenameCategoryCommandHandler(
            ICategoryRepository categoryRepository,
            IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<CategoryResponse>> Handle(
            RenameCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(CategoryId.Create(request.CategoryId));
            if (category is null)
                return Errors.Category.NotFound;

            var renameResult = category.Rename(request.Name);
            if (renameResult.IsError)
                return renameResult.Errors;

            _categoryRepository.Update(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new CategoryResponse(category.CategoryId.Value, category.Name);
        }
    }
}
