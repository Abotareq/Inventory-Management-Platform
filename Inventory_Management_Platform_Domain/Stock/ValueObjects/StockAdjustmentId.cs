using Inventory_Management_Platform.Domain.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Domain.Stock.ValueObjects
{
    public sealed class StockAdjustmentId : ValueObject
    {
        public Guid Value { get; }

        private StockAdjustmentId(Guid value)
        {
            Value = value;
        }

        public static StockAdjustmentId CreateUnique()
        {
            return new StockAdjustmentId(Guid.NewGuid());
        }

        public static StockAdjustmentId Create(Guid value)
        {
            return new StockAdjustmentId(value);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
