using ErrorOr;
using Inventory_Management_Platform.Application.Common.Interfaces.Persistence;
using Inventory_Management_Platform.Contracts.Category;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Categories.Queries.GetCategories
{
    public sealed class GetCategoriesQueryHandler
        : IRequestHandler<GetCategoriesQuery, ErrorOr<List<CategoryResponse>>>
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetCategoriesQueryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<ErrorOr<List<CategoryResponse>>> Handle(
            GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await _categoryRepository.GetAllAsync();

            var response = categories
                .Select(c => new CategoryResponse(c.CategoryId.Value, c.Name))
                .ToList();

            return response;
        }
    }
}

