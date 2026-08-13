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

            await SeedUserAsync(userManager, "admin@example.com", "AdminPass123!", "Administrator");
            await SeedUserAsync(userManager, "operator@example.com", "OperatorPass123!", "WarehouseOperator");
            await SeedUserAsync(userManager, "manager@example.com", "ManagerPass123!", "Manager");
        }

        private static async Task SeedUserAsync(
            UserManager<ApplicationUser> userManager, string email, string password, string role)
        {
            var existing = await userManager.FindByEmailAsync(email);
            if (existing is not null)
                return;

            var user = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, role);
            }
        }
    }
}
