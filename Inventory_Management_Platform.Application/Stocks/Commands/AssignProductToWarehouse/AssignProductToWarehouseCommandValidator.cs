using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Stocks.Commands.AssignProductToWarehouse
{
  
    public sealed class AssignProductToWarehouseCommandValidator
        : AbstractValidator<AssignProductToWarehouseCommand>
    {
        public AssignProductToWarehouseCommandValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty();
            RuleFor(x => x.WarehouseId).NotEmpty();
        }
    }
}
