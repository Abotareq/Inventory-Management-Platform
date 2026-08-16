using Inventory_Management_Platform.Domain.Product.ValueObjects;
using Inventory_Management_Platform.Domain.Stock;
using Inventory_Management_Platform.Domain.Stock.Entites;
using Inventory_Management_Platform.Domain.Stock.ValueObjects;
using Inventory_Management_Platform.Domain.Warehouse.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Common.Interfaces.Persistence
{
    public interface IStockRepository
    {
        Task<Stock?> GetByIdAsync(StockId id);
        Task<Stock?> GetByProductAndWarehouseAsync(ProductId productId, WarehouseId warehouseId);
        Task<(List<Stock> Items, int TotalCount)> GetByWarehouseAsync(
            WarehouseId warehouseId, int pageNumber, int pageSize);
        Task<(List<Stock> Items, int TotalCount)> GetByProductAsync(
            ProductId productId, int pageNumber, int pageSize);
        Task AddAsync(Stock stock);
        Task<bool> ExistsAsync(ProductId productId, WarehouseId warehouseId);
        Task AddAdjustmentAsync(StockId stockId, StockAdjustment adjustment);
        Task<(List<StockAdjustment> Items, int TotalCount)> GetAdjustmentHistoryAsync(
            StockId stockId, int pageNumber, int pageSize);
    }
}
