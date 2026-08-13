using Inventory_Management_Platform.Application.Common.Interfaces.Authentication;
using Inventory_Management_Platform.Application.Common.Interfaces.Persistence;
using Inventory_Management_Platform.Infrastructure.Authintication;
using Inventory_Management_Platform.Infrastructure.Identity;
using Inventory_Management_Platform.Infrastructure.Persistence;
using Inventory_Management_Platform.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
namespace Inventory_Management_Platform.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<InventoryManagementPlatformDbContext>((sp, options) =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                 });
            //unit of work
            services.AddScoped<IUnitOfWork>(sp =>
               sp.GetRequiredService<InventoryManagementPlatformDbContext>());

            // Identity
            services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.Password.RequiredLength = 8;
                options.User.RequireUniqueEmail = true;
            })
                .AddRoles<IdentityRole<Guid>>()
                .AddEntityFrameworkStores<InventoryManagementPlatformDbContext>();
            // JWT settings binding
            services.Configure<JwtSettings>(
                configuration.GetSection(JwtSettings.SectionName));
            // Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            // Authentication services
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            return services;

        }
    }
}