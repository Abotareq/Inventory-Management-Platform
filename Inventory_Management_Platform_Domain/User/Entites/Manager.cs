using ErrorOr;
using Inventory_Management_Platform.Domain.User.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Domain.User.Entites
{
    public sealed class Manager:User
    {
        private Manager(UserId userId, string fullName, string email)
           : base(userId, fullName, email)
        {
        }

        private Manager() { }

        public static ErrorOr<Manager> Create(UserId userId, string fullName, string email)
        {
            var errors = ValidateBasicInfo(fullName, email);
            if (errors.Count > 0)
                return errors;

            return new Manager(userId, fullName, email);
        }
    }
}
