using ErrorOr;
using Inventory_Management_Platform.Application.Common.Exceptions;
using Inventory_Management_Platform.Application.Common.Interfaces.Persistence;
using Inventory_Management_Platform.Contracts.Category;
using Inventory_Management_Platform.Domain.Category.ValueObjects;
using Inventory_Management_Platform.Domain.DomainErrors;
using MediatR;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<RenameCategoryCommandHandler> _logger;

        public RenameCategoryCommandHandler(
            ICategoryRepository categoryRepository,
            IUnitOfWork unitOfWork,
            ILogger<RenameCategoryCommandHandler> logger)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ErrorOr<CategoryResponse>> Handle(
      RenameCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(CategoryId.Create(request.CategoryId));
            if (category is null)
                return Errors.Category.NotFound;

            if (!string.Equals(category.Name, request.Name, StringComparison.Ordinal)
                && await _categoryRepository.ExistsByNameAsync(request.Name))
            {
                return Errors.Category.NameAlreadyExists;
            }

            var renameResult = category.Rename(request.Name);
            if (renameResult.IsError)
                return renameResult.Errors;

            try
            {
                _categoryRepository.Update(category);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
            catch (UniqueConstraintViolationException)
            {
                _logger.LogWarning("Unique constraint violation renaming category. CategoryId: {CategoryId}, Name: {Name}", request.CategoryId, request.Name);
                return Errors.Category.NameAlreadyExists;
            }

            return new CategoryResponse(category.CategoryId.Value, category.Name);
        }
    }
}
