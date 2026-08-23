using Inventory_Management_Platform.Domain.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Domain.Order.ValueObjects
{
    public sealed class OrderHistoryId : ValueObject
    {
        public Guid Value { get; }

        private OrderHistoryId(Guid value)
        {
            Value = value;
        }

        public static OrderHistoryId CreateUnique() => new(Guid.NewGuid());
        public static OrderHistoryId Create(Guid value) => new(value);

        protected  override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
