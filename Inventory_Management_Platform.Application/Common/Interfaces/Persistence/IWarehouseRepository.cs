using Inventory_Management_Platform.Domain.Warehouse;
using Inventory_Management_Platform.Domain.Warehouse.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Common.Interfaces.Persistence
{
    public interface IWarehouseRepository
    {
        Task<Warehouse?> GetByIdAsync(WarehouseId id);
        Task<List<Warehouse>> GetAllAsync();
        Task AddAsync(Warehouse warehouse);
        void Update(Warehouse warehouse);
        Task<bool> ExistsAsync(WarehouseId id);
    }
}
