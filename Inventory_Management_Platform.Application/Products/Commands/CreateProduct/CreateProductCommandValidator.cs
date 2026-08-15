using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Products.Commands.CreateProduct
{
    public sealed class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Sku).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Description).MaximumLength(1000);
        }
    }
}
