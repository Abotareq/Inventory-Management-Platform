using ErrorOr;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Domain.DomainErrors
{
    public static partial class Errors
    {
        public static class Stock
        {
            public static Error InvalidAdjustmentAmount => Error.Validation(
                "Stock.InvalidAdjustmentAmount",
                "Adjustment amount must be greater than zero.");

            public static Error ReasonIsRequired => Error.Validation(
                "Stock.ReasonIsRequired",
                "A reason is required for stock adjustments.");

            public static Error InsufficientStock => Error.Validation(
                "Stock.InsufficientStock",
                "Stock quantity cannot go below zero.");

            public static Error NotFound => Error.NotFound(
                "Stock.NotFound",
                "Stock record was not found.");

            public static Error AlreadyExists => Error.Conflict(
                "Stock.AlreadyExists",
                "This product is already assigned to this warehouse.");
        }
    }
   
}
