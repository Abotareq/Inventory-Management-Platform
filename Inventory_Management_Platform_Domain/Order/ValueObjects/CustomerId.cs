using Inventory_Management_Platform.Domain.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Domain.Order.ValueObjects
{
    public sealed class CustomerId : ValueObject
    {
        public Guid Value { get; }

        private CustomerId(Guid value)
        {
            Value = value;
        }

        public static CustomerId CreateUnique() => new(Guid.NewGuid());
        public static CustomerId Create(Guid value) => new(value);

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
