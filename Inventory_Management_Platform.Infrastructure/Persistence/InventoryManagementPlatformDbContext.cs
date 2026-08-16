using Inventory_Management_Platform.Application.Common.Interfaces.Persistence;
using Inventory_Management_Platform.Domain.Category;
using Inventory_Management_Platform.Domain.Product;
using Inventory_Management_Platform.Domain.Stock;
using Inventory_Management_Platform.Domain.Stock.Entites;
using Inventory_Management_Platform.Domain.User;
using Inventory_Management_Platform.Domain.Warehouse;
using Inventory_Management_Platform.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Infrastructure.Persistence
{
    public sealed class InventoryManagementPlatformDbContext
        : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>, IUnitOfWork
    {
        public InventoryManagementPlatformDbContext(DbContextOptions<InventoryManagementPlatformDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Warehouse> Warehouses => Set<Warehouse>();
        public DbSet<Stock> Stocks => Set<Stock>();
        public DbSet<StockAdjustment> StockAdjustments => Set<StockAdjustment>();
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); // required — configures Identity's own tables

            builder.ApplyConfigurationsFromAssembly(typeof(InventoryManagementPlatformDbContext).Assembly);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }

    }
}
