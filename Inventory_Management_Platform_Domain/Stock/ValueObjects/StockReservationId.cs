using Inventory_Management_Platform.Domain.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Domain.Stock.ValueObjects
{
    public sealed class StockReservationId : ValueObject
    {
        public Guid Value { get; }

        private StockReservationId(Guid value)
        {
            Value = value;
        }

        public static StockReservationId CreateUnique() => new(Guid.NewGuid());
        public static StockReservationId Create(Guid value) => new(value);

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
