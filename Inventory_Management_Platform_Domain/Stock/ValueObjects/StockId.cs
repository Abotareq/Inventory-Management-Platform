using Inventory_Management_Platform.Domain.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Domain.Stock.ValueObjects
{
    public sealed class StockId : ValueObject
    {
        public Guid Value { get; }

        private StockId(Guid value)
        {
            Value = value;
        }

        public static StockId CreateUnique()
        {
            return new StockId(Guid.NewGuid());
        }

        public static StockId Create(Guid value)
        {
            return new StockId(value);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
