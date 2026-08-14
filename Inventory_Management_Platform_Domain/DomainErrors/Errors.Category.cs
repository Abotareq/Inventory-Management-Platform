using ErrorOr;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Domain.DomainErrors
{
    public static partial class Errors
    {
        public static class Category
        {
            public static Error NameIsRequired => Error.Validation(
                "Category.NameIsRequired",
                "Category name is required.");

            public static Error NameTooLong => Error.Validation(
                "Category.NameTooLong",
                "Category name must not exceed 100 characters.");

            public static Error NotFound => Error.NotFound(
                "Category.NotFound",
                "Category was not found.");
        }
    }
}
