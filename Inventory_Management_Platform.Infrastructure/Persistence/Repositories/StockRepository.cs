using Inventory_Management_Platform.Application.Common.Interfaces.Persistence;
using Inventory_Management_Platform.Domain.Order.Entites;
using Inventory_Management_Platform.Domain.Product.ValueObjects;
using Inventory_Management_Platform.Domain.Stock;
using Inventory_Management_Platform.Domain.Stock.Entites;
using Inventory_Management_Platform.Domain.Stock.ValueObjects;
using Inventory_Management_Platform.Domain.Warehouse.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
namespace Inventory_Management_Platform.Infrastructure.Persistence.Repositories
{
    public sealed class StockRepository : IStockRepository
    {
        private readonly InventoryManagementPlatformDbContext _dbContext;

        public StockRepository(InventoryManagementPlatformDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Stock?> GetByIdAsync(StockId id)
        {
            return await _dbContext.Stocks
                .FirstOrDefaultAsync(s => s.StockId == id);
        }

        public async Task<Stock?> GetByProductAndWarehouseAsync(
            ProductId productId, WarehouseId warehouseId)
        {
            return await _dbContext.Stocks
                .FirstOrDefaultAsync(s => s.ProductId == productId && s.WarehouseId == warehouseId);
        }

        public async Task<(List<Stock> Items, int TotalCount)> GetByWarehouseAsync(
            WarehouseId warehouseId, int pageNumber, int pageSize)
        {
            var query = _dbContext.Stocks
                .Where(s => s.WarehouseId == warehouseId);

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<(List<Stock> Items, int TotalCount)> GetByProductAsync(
            ProductId productId, int pageNumber, int pageSize)
        {
            var query = _dbContext.Stocks
                .Where(s => s.ProductId == productId);

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task AddAsync(Stock stock)
        {
            await _dbContext.Stocks.AddAsync(stock);
        }

        public async Task<bool> ExistsAsync(ProductId productId, WarehouseId warehouseId)
        {
            return await _dbContext.Stocks
                .AnyAsync(s => s.ProductId == productId && s.WarehouseId == warehouseId);
        }

        public async Task AddAdjustmentAsync(StockId stockId, StockAdjustment adjustment)
        {
            _dbContext.Set<StockAdjustment>().Add(adjustment);
            await Task.CompletedTask;
        }

        public async Task<(List<StockAdjustment> Items, int TotalCount)> GetAdjustmentHistoryAsync(
            StockId stockId, int pageNumber, int pageSize)
        {
            var query = _dbContext.Set<StockAdjustment>()
                .Where(a => a.StockId == stockId)
                .OrderByDescending(a => a.Timestamp);

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }
        public async Task AddReservationAsync(StockReservation reservation)
        {
            await _dbContext.Set<StockReservation>().AddAsync(reservation);
        }
        public async Task<(List<StockReservation> Items, int TotalCount)> GetReservationHistoryAsync(
    StockId stockId, int pageNumber, int pageSize)
        {
            var query = _dbContext.Set<StockReservation>()
                .Where(r => r.StockId == stockId)
                .OrderByDescending(r => r.Timestamp);

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }
        public void Delete(Stock stock)
        {
            _dbContext.Stocks.Remove(stock);
        }

        public async Task<bool> HasOrderItemsAsync(ProductId productId, WarehouseId warehouseId)
        {
            return await _dbContext.Set<OrderItem>()
                .AnyAsync(i => i.ProductId == productId && i.WarehouseId == warehouseId);
        }
    }
}
