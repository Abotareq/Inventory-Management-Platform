using ErrorOr;
using Inventory_Management_Platform.Domain.User.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Domain.User.Entites
{
    public sealed class Administrator : User
    {
        private Administrator(UserId userId, string fullName, string email)
           : base(userId, fullName, email)
        {
        }

        private Administrator() { }

        public static ErrorOr<Administrator> Create(UserId userId, string fullName, string email)
        {
            var errors = ValidateBasicInfo(fullName, email);
            if (errors.Count > 0)
                return errors;

            return new Administrator(userId, fullName, email);
        }
    }
}
