using Inventory_Management_Platform.Domain.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Domain.Order.ValueObjects
{

    public sealed class OrderItemId : ValueObject
    {
        public Guid Value { get; }

        private OrderItemId(Guid value)
        {
            Value = value;
        }

        public static OrderItemId CreateUnique() => new(Guid.NewGuid());
        public static OrderItemId Create(Guid value) => new(value);

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
