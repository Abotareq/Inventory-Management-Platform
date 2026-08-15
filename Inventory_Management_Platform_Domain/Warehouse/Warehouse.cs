using ErrorOr;
using Inventory_Management_Platform.Domain.Common.Models;
using Inventory_Management_Platform.Domain.DomainErrors;
using Inventory_Management_Platform.Domain.Warehouse.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Domain.Warehouse
{
    public sealed class Warehouse : AggregateRoot
    {
        public WarehouseId WarehouseId { get; private set; }
        public string Name { get; private set; }
        public string Location { get; private set; }

        private Warehouse(WarehouseId warehouseId, string name, string location)
            : base(warehouseId.Value)
        {
            WarehouseId = warehouseId;
            Name = name;
            Location = location;
        }

        private Warehouse() { }

        public static ErrorOr<Warehouse> Create(string name, string location)
        {
            var errors = Validate(name, location);
            if (errors.Count > 0)
                return errors;

            return new Warehouse(WarehouseId.CreateUnique(), name, location);
        }

        public ErrorOr<Updated> UpdateDetails(string name, string location)
        {
            var errors = Validate(name, location);
            if (errors.Count > 0)
                return errors;

            Name = name;
            Location = location;

            return Result.Updated;
        }

        private static List<Error> Validate(string name, string location)
        {
            var errors = new List<Error>();

            if (string.IsNullOrWhiteSpace(name))
                errors.Add(Errors.Warehouse.NameIsRequired);
            else if (name.Length > 200)
                errors.Add(Errors.Warehouse.NameTooLong);

            if (string.IsNullOrWhiteSpace(location))
                errors.Add(Errors.Warehouse.LocationIsRequired);
            else if (location.Length > 300)
                errors.Add(Errors.Warehouse.LocationTooLong);

            return errors;
        }
    }
}
