using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Categories.Commands.RenameCategory
{
    public sealed class RenameCategoryCommandValidator : AbstractValidator<RenameCategoryCommand>
    {
        public RenameCategoryCommandValidator()
        {
            RuleFor(x => x.CategoryId).NotEmpty();
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        }
    }
}
