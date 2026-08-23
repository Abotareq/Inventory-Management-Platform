using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Orders.Commands.CancelOrder
{
    public sealed class CancelOrderCommandValidator : AbstractValidator<CancelOrderCommand>
    {
        public CancelOrderCommandValidator()
        {
            RuleFor(x => x.OrderId).NotEmpty();
        }
    }
}
