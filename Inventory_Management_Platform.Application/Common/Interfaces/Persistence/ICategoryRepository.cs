using Inventory_Management_Platform.Domain.Category;
using Inventory_Management_Platform.Domain.Category.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Common.Interfaces.Persistence
{
    public interface ICategoryRepository
    {
        Task<Category?> GetByIdAsync(CategoryId id);
        Task<List<Category>> GetAllAsync();
        Task AddAsync(Category category);
        void Update(Category category);
        Task<bool> ExistsAsync(CategoryId id);
        Task<bool> HasProductsAsync(CategoryId id);
        void Delete(Category category);
    }
}
