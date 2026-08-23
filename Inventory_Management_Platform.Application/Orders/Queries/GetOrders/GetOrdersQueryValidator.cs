using FluentValidation;
using Inventory_Management_Platform.Domain.Order.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Orders.Queries.GetOrders
{
    public sealed class GetOrdersQueryValidator : AbstractValidator<GetOrdersQuery>
    {
        public GetOrdersQueryValidator()
        {
            RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1);
            RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
            RuleFor(x => x.Status)
                .Must(s => s is null || Enum.TryParse<OrderStatus>(s, ignoreCase: true, out _))
                .WithMessage("Status must be a valid order status.");
        }
    }
}
