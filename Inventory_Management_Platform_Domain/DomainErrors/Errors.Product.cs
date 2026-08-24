using ErrorOr;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Domain.DomainErrors
{
    public static partial class Errors
    {
        public static class Product
        {
            public static Error NameIsRequired => Error.Validation(
                "Product.NameIsRequired",
                "Product name is required.");

            public static Error NameTooLong => Error.Validation(
                "Product.NameTooLong",
                "Product name must not exceed 200 characters.");

            public static Error SkuIsRequired => Error.Validation(
                "Product.SkuIsRequired",
                "Product SKU is required.");

            public static Error SkuTooLong => Error.Validation(
                "Product.SkuTooLong",
                "Product SKU must not exceed 50 characters.");

            public static Error NotFound => Error.NotFound(
                "Product.NotFound",
                "Product was not found.");
            public static Error SkuAlreadyExists => Error.Conflict(
    "Product.SkuAlreadyExists",
    "A product with this SKU already exists.");
            public static Error HasStock => Error.Conflict(
        "Product.HasStock",
        "Product cannot be deleted because it has stock records assigned to it.");
            public static Error InvalidPrice => Error.Validation(
    "Product.InvalidPrice",
    "Product price cannot be negative.");
            public static Error HasOrderItems => Error.Conflict(
    "Product.HasOrderItems",
    "Product cannot be deleted because it is referenced by one or more orders.");
        }

    }
}
