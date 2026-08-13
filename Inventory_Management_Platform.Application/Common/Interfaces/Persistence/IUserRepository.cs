using Inventory_Management_Platform.Domain.User;
using Inventory_Management_Platform.Domain.User.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Common.Interfaces.Persistence
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(UserId userId);
        Task<User?> GetByEmailAsync(string email);
        Task AddAsync(User user);
        void Delete(User user);
    }
}
