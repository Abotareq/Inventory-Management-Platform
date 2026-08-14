using Inventory_Management_Platform.Domain.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Domain.Category.ValueObjects
{
    public sealed class CategoryId : ValueObject
    {
        public Guid Value { get; }

        private CategoryId(Guid value)
        {
            Value = value;
        }

        public static CategoryId CreateUnique()
        {
            return new CategoryId(Guid.NewGuid());
        }

        public static CategoryId Create(Guid value)
        {
            return new CategoryId(value);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
