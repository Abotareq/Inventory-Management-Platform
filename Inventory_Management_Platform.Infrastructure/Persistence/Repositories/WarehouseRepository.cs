using Inventory_Management_Platform.Domain.Warehouse;
using Inventory_Management_Platform.Domain.Warehouse.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Inventory_Management_Platform.Application.Common.Interfaces.Persistence;
namespace Inventory_Management_Platform.Infrastructure.Persistence.Repositories
{
   
        public sealed class WarehouseRepository : IWarehouseRepository
        {
            private readonly InventoryManagementPlatformDbContext _dbContext;

            public WarehouseRepository(InventoryManagementPlatformDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<Warehouse?> GetByIdAsync(WarehouseId id )
            {
                return await _dbContext.Warehouses
                    .FirstOrDefaultAsync(w => w.WarehouseId == id);
            }

            public async Task<List<Warehouse>> GetAllAsync()
            {
                return await _dbContext.Warehouses.ToListAsync();
            }

            public async Task AddAsync(Warehouse warehouse)
            {
                await _dbContext.Warehouses.AddAsync(warehouse);
            }

            public void Update(Warehouse warehouse)
            {
                _dbContext.Warehouses.Update(warehouse);
            }

            public async Task<bool> ExistsAsync(WarehouseId id)
            {
                return await _dbContext.Warehouses
                    .AnyAsync(w => w.WarehouseId == id);
            }
        public void Delete(Warehouse warehouse)
        {
            _dbContext.Warehouses.Remove(warehouse);
        }

        public async Task<bool> HasStockAsync(WarehouseId id)
        {
            return await _dbContext.Stocks
                .AnyAsync(s => s.WarehouseId == id);
        }
    }
    
}
