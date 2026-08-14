using ErrorOr;
using Inventory_Management_Platform.Application.Common.Interfaces.Persistence;
using Inventory_Management_Platform.Contracts.Category;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using DomainCategory = Inventory_Management_Platform.Domain.Category.Category;
namespace Inventory_Management_Platform.Application.Categories.Commands.CreateCategory
{
    public sealed class CreateCategoryCommandHandler
        : IRequestHandler<CreateCategoryCommand, ErrorOr<CategoryResponse>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateCategoryCommandHandler(
            ICategoryRepository categoryRepository,
            IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<CategoryResponse>> Handle(
      CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var categoryResult = DomainCategory.Create(request.Name);
            if (categoryResult.IsError)
                return categoryResult.Errors;

            var category = categoryResult.Value;

            await _categoryRepository.AddAsync(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new CategoryResponse(category.CategoryId.Value, category.Name);
        }
    }
}
