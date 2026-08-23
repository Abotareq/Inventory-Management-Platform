using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Orders.Commands.BeginProcessing
{

    public sealed class BeginProcessingCommandValidator : AbstractValidator<BeginProcessingCommand>
    {
        public BeginProcessingCommandValidator()
        {
            RuleFor(x => x.OrderId).NotEmpty();
        }
    }
}
