using Inventory_Management_Platform.Domain.Product;
using Inventory_Management_Platform.Domain.Product.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Common.Interfaces.Persistence
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(ProductId id);
        Task<(List<Product> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize);
        Task AddAsync(Product product);
        void Update(Product product);
        Task<bool> ExistsBySkuAsync(string sku);
        void Delete(Product product);
        Task<bool> HasStockAsync(ProductId id);
        Task<bool> HasOrderItemsAsync(ProductId id);
    }
}
