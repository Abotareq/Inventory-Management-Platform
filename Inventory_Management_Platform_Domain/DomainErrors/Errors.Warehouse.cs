using ErrorOr;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Domain.DomainErrors
{
    public static partial class Errors
    {
        public static class Warehouse
        {
            public static Error NameIsRequired => Error.Validation(
                "Warehouse.NameIsRequired",
                "Warehouse name is required.");

            public static Error NameTooLong => Error.Validation(
                "Warehouse.NameTooLong",
                "Warehouse name must not exceed 200 characters.");

            public static Error LocationIsRequired => Error.Validation(
                "Warehouse.LocationIsRequired",
                "Warehouse location is required.");

            public static Error LocationTooLong => Error.Validation(
                "Warehouse.LocationTooLong",
                "Warehouse location must not exceed 300 characters.");

            public static Error NotFound => Error.NotFound(
                "Warehouse.NotFound",
                "Warehouse was not found.");
        }
    }
}
