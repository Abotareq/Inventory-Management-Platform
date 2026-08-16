using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Stocks.Queries.GetStockAdjustmentHistory
{
    public sealed class GetStockAdjustmentHistoryQueryValidator
          : AbstractValidator<GetStockAdjustmentHistoryQuery>
    {
        public GetStockAdjustmentHistoryQueryValidator()
        {
            RuleFor(x => x.StockId).NotEmpty();
            RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1);
            RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
        }
    }
}
