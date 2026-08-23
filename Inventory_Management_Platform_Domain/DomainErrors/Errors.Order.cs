using ErrorOr;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Domain.DomainErrors
{
    public static partial class Errors
    {
        public static class Order
        {
            public static Error NotFound => Error.NotFound(
                "Order.NotFound", "Order was not found.");

            public static Error InvalidQuantity => Error.Validation(
                "Order.InvalidQuantity", "Order item quantity must be greater than zero.");

            public static Error InvalidUnitPrice => Error.Validation(
                "Order.InvalidUnitPrice", "Order item unit price cannot be negative.");

            public static Error EmptyOrder => Error.Validation(
                "Order.EmptyOrder", "Order must contain at least one item before it can be submitted.");

            public static Error CannotModifyNonDraftOrder => Error.Validation(
                "Order.CannotModifyNonDraftOrder", "Items can only be added while the order is in Draft status.");

            public static Error InvalidStatusTransition => Error.Conflict(
                "Order.InvalidStatusTransition", "This status transition is not allowed from the order's current state.");
            public static Error ConcurrencyConflict => Error.Conflict(
    "Order.ConcurrencyConflict",
    "This order was modified by another user. Please retry your request.");
        }
    }
}
