using ErrorOr;

namespace Inventory_Management_Platform.Domain.DomainErrors
{
    public static partial class Errors
    {
        public static class User
        {
            public static Error FullNameIsRequired => Error.Validation(
                code: "User.FullNameIsRequired",
                description: "Full name is required.");

            public static Error EmailIsRequired => Error.Validation(
                code: "User.EmailIsRequired",
                description: "Email is required.");

            public static Error InvalidEmailFormat => Error.Validation(
                code: "User.InvalidEmailFormat",
                description: "Email format is invalid.");
            public static Error InvalidRole => Error.Validation(
    "User.InvalidRole",
    "The specified role is not valid.");
        }
    }
}

