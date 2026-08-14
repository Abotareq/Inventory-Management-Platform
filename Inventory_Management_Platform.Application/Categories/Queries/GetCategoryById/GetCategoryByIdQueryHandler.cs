using ErrorOr;
using Inventory_Management_Platform.Application.Common.Interfaces.Persistence;
using Inventory_Management_Platform.Contracts.Category;
using Inventory_Management_Platform.Domain.Category.ValueObjects;
using Inventory_Management_Platform.Domain.DomainErrors;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Categories.Queries.GetCategoryById
{

    public sealed class GetCategoryByIdQueryHandler
        : IRequestHandler<GetCategoryByIdQuery, ErrorOr<CategoryResponse>>
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetCategoryByIdQueryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<ErrorOr<CategoryResponse>> Handle(
            GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(CategoryId.Create(request.CategoryId));
            if (category is null)
                return Errors.Category.NotFound;

            return new CategoryResponse(category.CategoryId.Value, category.Name);
        }
    }
}
