using Inventory_Management_Platform.Domain.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Domain.Product.ValueObjects
{
    public sealed class ProductId : ValueObject
    {
        public Guid Value { get; }

        private ProductId(Guid value)
        {
            Value = value;
        }

        public static ProductId CreateUnique()
        {
            return new ProductId(Guid.NewGuid());
        }

        public static ProductId Create(Guid value)
        {
            return new ProductId(value);
        }

          protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
