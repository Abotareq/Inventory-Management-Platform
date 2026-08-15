using Inventory_Management_Platform.Domain.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Domain.Warehouse.ValueObjects
{
    public sealed class WarehouseId : ValueObject
    {
        public Guid Value { get; }

        private WarehouseId(Guid value)
        {
            Value = value;
        }

        public static WarehouseId CreateUnique()
        {
            return new WarehouseId(Guid.NewGuid());
        }

        public static WarehouseId Create(Guid value)
        {
            return new WarehouseId(value);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
        
    }
}
