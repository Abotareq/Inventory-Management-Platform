using Inventory_Management_Platform.Application.Common.Interfaces.Persistence;
using Inventory_Management_Platform.Domain.User;
using Inventory_Management_Platform.Domain.User.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
namespace Inventory_Management_Platform.Infrastructure.Persistence.Repositories
{
    public sealed class UserRepository : IUserRepository
    {
        private readonly InventoryManagementPlatformDbContext _dbContext;

        public UserRepository(InventoryManagementPlatformDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User?> GetByIdAsync(UserId userId)
        {
            return await _dbContext.Users
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task AddAsync(User user)
        {
            await _dbContext.Users.AddAsync(user);
        }

        public void Delete(User user)
        {
            _dbContext.Users.Remove(user);
        }
    }
}
