using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Stocks.Queries.GetStockByProduct
{
    public sealed class GetStockByProductQueryValidator : AbstractValidator<GetStockByProductQuery>
    {
        public GetStockByProductQueryValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty();
            RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1);
            RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
        }
    }
}
