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
        Task<List<Product>> GetAllAsync();
        Task AddAsync(Product product);
        void Update(Product product);
        Task<bool> ExistsBySkuAsync(string sku);
    }
}
