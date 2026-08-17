using Inventory_Management_Platform.Application.Common.Interfaces.Persistence;
using Inventory_Management_Platform.Domain.Product;
using Inventory_Management_Platform.Domain.Product.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
namespace Inventory_Management_Platform.Infrastructure.Persistence.Repositories
{
    public sealed class ProductRepository : IProductRepository
    {
        private readonly InventoryManagementPlatformDbContext _dbContext;

        public ProductRepository(InventoryManagementPlatformDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Product?> GetByIdAsync(ProductId id)
        {
            return await _dbContext.Products
                .FirstOrDefaultAsync(p => p.ProductId == id);
        }

        public async Task<(List<Product> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize)
        {
            var query = _dbContext.Products.OrderBy(p => p.Name);

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task AddAsync(Product product)
        {
            await _dbContext.Products.AddAsync(product);
        }

        public void Update(Product product)
        {
            _dbContext.Products.Update(product);
        }

        public async Task<bool> ExistsBySkuAsync(string sku)
        {
            return await _dbContext.Products
                .AnyAsync(p => p.Sku == sku);
        }
        public void Delete(Product product)
        {
            _dbContext.Products.Remove(product);
        }

        public async Task<bool> HasStockAsync(ProductId id)
        {
            return await _dbContext.Stocks
                .AnyAsync(s => s.ProductId == id);
        }
    }
}
