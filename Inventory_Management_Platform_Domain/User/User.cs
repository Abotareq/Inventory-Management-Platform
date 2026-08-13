using ErrorOr;
using Inventory_Management_Platform.Domain.Common.Models;
using Inventory_Management_Platform.Domain.DomainErrors;
using Inventory_Management_Platform.Domain.User.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Domain.User
{
    public abstract class User : AggregateRoot
    {
        public UserId UserId { get; private set; }
        public string FullName { get; private set; }
        public string Email { get; private set; }

        protected User(UserId userId, string fullName, string email)
            : base(userId.Value)
        {
            UserId = userId;
            FullName = fullName;
            Email = email;
        }
        protected static List<Error> ValidateBasicInfo(string fullName, string email)
        {
            var errors = new List<Error>();

            if (string.IsNullOrWhiteSpace(fullName))
                errors.Add(Errors.User.FullNameIsRequired);

            if (string.IsNullOrWhiteSpace(email))
                errors.Add(Errors.User.EmailIsRequired);
            else if (!email.Contains('@'))
                errors.Add(Errors.User.InvalidEmailFormat);

            return errors;
        }
        protected User() { }

    }
}