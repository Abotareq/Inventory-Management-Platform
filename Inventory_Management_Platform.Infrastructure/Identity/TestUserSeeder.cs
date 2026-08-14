using Inventory_Management_Platform.Application.Common.Interfaces.Persistence;
using Inventory_Management_Platform.Domain.User;
using Inventory_Management_Platform.Domain.User.Entites;
using Inventory_Management_Platform.Domain.User.ValueObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Infrastructure.Identity
{
    public static class TestUserSeeder
    {
        public static async Task SeedTestUsersAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var userRepository = serviceProvider.GetRequiredService<IUserRepository>();
            var unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();

            await SeedUserAsync(userManager, userRepository, unitOfWork,
                "Admin User", "admin@example.com", "AdminPass123!", "Administrator");
            await SeedUserAsync(userManager, userRepository, unitOfWork,
                "Operator User", "operator@example.com", "OperatorPass123!", "WarehouseOperator");
            await SeedUserAsync(userManager, userRepository, unitOfWork,
                "Manager User", "manager@example.com", "ManagerPass123!", "Manager");
        }

        private static async Task SeedUserAsync(
            UserManager<ApplicationUser> userManager,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            string fullName, string email, string password, string role)
        {
            var existing = await userManager.FindByEmailAsync(email);
            if (existing is not null)
                return;

            var userId = UserId.CreateUnique();

            var identityUser = new ApplicationUser
            {
                Id = userId.Value,
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };

            var identityResult = await userManager.CreateAsync(identityUser, password);
            if (!identityResult.Succeeded)
                return;

            await userManager.AddToRoleAsync(identityUser, role);

            User? domainUser = role switch
            {
                "Administrator" => Administrator.Create(userId, fullName, email).Value,
                "WarehouseOperator" => WarehouseOperator.Create(userId, fullName, email).Value,
                "Manager" => Manager.Create(userId, fullName, email).Value,
                _ => null
            };

            if (domainUser is null)
                return;

            await userRepository.AddAsync(domainUser);
            await unitOfWork.SaveChangesAsync(CancellationToken.None);
        }
    }
}
