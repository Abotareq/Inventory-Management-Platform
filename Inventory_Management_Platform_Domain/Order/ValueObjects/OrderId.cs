using Inventory_Management_Platform.Domain.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Domain.Order.ValueObjects
{

    public sealed class OrderId : ValueObject
    {
        public Guid Value { get; }

        private OrderId(Guid value)
        {
            Value = value;
        }

        public static OrderId CreateUnique() => new(Guid.NewGuid());
        public static OrderId Create(Guid value) => new(value);

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
