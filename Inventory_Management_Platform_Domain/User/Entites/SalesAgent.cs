using ErrorOr;
using Inventory_Management_Platform.Domain.User.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Domain.User.Entites
{

    public sealed class SalesAgent : User
    {
        private SalesAgent(UserId userId, string fullName, string email)
            : base(userId, fullName, email)
        {
        }

        private SalesAgent() { }

        public static ErrorOr<SalesAgent> Create(UserId userId, string fullName, string email)
        {
            var errors = ValidateBasicInfo(fullName, email);
            if (errors.Count > 0)
                return errors;

            return new SalesAgent(userId, fullName, email);
        }
    }
}
