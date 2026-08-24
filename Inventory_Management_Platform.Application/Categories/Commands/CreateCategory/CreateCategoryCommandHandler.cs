using ErrorOr;
using Inventory_Management_Platform.Application.Common.Exceptions;
using Inventory_Management_Platform.Application.Common.Interfaces.Persistence;
using Inventory_Management_Platform.Contracts.Category;
using Inventory_Management_Platform.Domain.DomainErrors;
using MediatR;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<CreateCategoryCommandHandler> _logger;

        public CreateCategoryCommandHandler(
            ICategoryRepository categoryRepository,
            IUnitOfWork unitOfWork,
            ILogger<CreateCategoryCommandHandler> logger)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ErrorOr<CategoryResponse>> Handle(
     CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            if (await _categoryRepository.ExistsByNameAsync(request.Name))
                return Errors.Category.NameAlreadyExists;

            var categoryResult = DomainCategory.Create(request.Name);
            if (categoryResult.IsError)
                return categoryResult.Errors;

            var category = categoryResult.Value;

            try
            {
                await _categoryRepository.AddAsync(category);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
            catch (UniqueConstraintViolationException)
            {
                _logger.LogWarning(
       "Unique constraint violation creating category. Name: {Name}",
       request.Name);
                return Errors.Category.NameAlreadyExists;
            }

            return new CategoryResponse(category.CategoryId.Value, category.Name);
        }
    }
}
