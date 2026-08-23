using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Orders.Queries.GetOrderHistory
{
    public sealed class GetOrderHistoryQueryValidator : AbstractValidator<GetOrderHistoryQuery>
    {
        public GetOrderHistoryQueryValidator()
        {
            RuleFor(x => x.OrderId).NotEmpty();
            RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1);
            RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
        }
    }
}
