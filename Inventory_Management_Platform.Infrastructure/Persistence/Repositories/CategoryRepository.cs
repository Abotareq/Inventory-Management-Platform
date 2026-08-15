using Inventory_Management_Platform.Application.Common.Interfaces.Persistence;
using Inventory_Management_Platform.Domain.Category;
using Inventory_Management_Platform.Domain.Category.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
namespace Inventory_Management_Platform.Infrastructure.Persistence.Repositories
{
    public sealed class CategoryRepository : ICategoryRepository
    {
        private readonly InventoryManagementPlatformDbContext _dbContext;

        public CategoryRepository(InventoryManagementPlatformDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Category?> GetByIdAsync(CategoryId id)
        {
            return await _dbContext.Categories
                .FirstOrDefaultAsync(c => c.CategoryId == id);
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _dbContext.Categories.ToListAsync();
        }

        public async Task AddAsync(Category category)
        {
            await _dbContext.Categories.AddAsync(category);
        }

        public void Update(Category category)
        {
            _dbContext.Categories.Update(category);
        }

        public async Task<bool> ExistsAsync(CategoryId id)
        {
            return await _dbContext.Categories
                .AnyAsync(c => c.CategoryId == id);
        }
        public async Task<bool> HasProductsAsync(CategoryId id)
        {
            return await _dbContext.Products
                .AnyAsync(p => p.CategoryId == id);
        }
        public void Delete(Category category)
        {
            _dbContext.Categories.Remove(category);
        }
    }
}
