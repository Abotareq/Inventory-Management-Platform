using Inventory_Management_Platform.Application.Common.Interfaces.Authentication;
using Inventory_Management_Platform.Application.Common.Interfaces.Persistence;
using Inventory_Management_Platform.Application.Common.Interfaces.Services;
using Inventory_Management_Platform.Infrastructure.Authintication;
using Inventory_Management_Platform.Infrastructure.Identity;
using Inventory_Management_Platform.Infrastructure.Persistence;
using Inventory_Management_Platform.Infrastructure.Persistence.Auditing;
using Inventory_Management_Platform.Infrastructure.Persistence.Interceptors;
using Inventory_Management_Platform.Infrastructure.Persistence.Repositories;
using Inventory_Management_Platform.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
                options.AddInterceptors(
                    sp.GetRequiredService<DomainEventsDispatchInterceptor>(),
                    sp.GetRequiredService<AuditInterceptor>());
            });
            services.AddScoped<DomainEventsDispatchInterceptor>();
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
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IWarehouseRepository, WarehouseRepository>();
            services.AddScoped<IStockRepository, StockRepository>();
            // Authentication services
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            //Auditing
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<AuditInterceptor>();
            services.AddScoped<IAuditLogRepository, AuditLogRepository>();
            return services;

        }
    }
}