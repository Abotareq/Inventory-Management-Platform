using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Category.Commands.CreateCategory
{
    public sealed class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        }
    }
}
