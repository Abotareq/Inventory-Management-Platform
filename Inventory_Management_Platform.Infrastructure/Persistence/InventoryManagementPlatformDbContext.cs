using Inventory_Management_Platform.Application.Common.Exceptions;
using Inventory_Management_Platform.Application.Common.Interfaces.Persistence;
using Inventory_Management_Platform.Application.Common.Models;
using Inventory_Management_Platform.Domain.Category;
using Inventory_Management_Platform.Domain.Order;
using Inventory_Management_Platform.Domain.Order.Entites;
using Inventory_Management_Platform.Domain.Product;
using Inventory_Management_Platform.Domain.Stock;
using Inventory_Management_Platform.Domain.Stock.Entites;
using Inventory_Management_Platform.Domain.User;
using Inventory_Management_Platform.Domain.Warehouse;
using Inventory_Management_Platform.Infrastructure.Identity;
using Inventory_Management_Platform.Infrastructure.Persistence.Auditing;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
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
        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<OrderHistory> OrderHistories => Set<OrderHistory>();
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); // required — configures Identity's own tables

            builder.ApplyConfigurationsFromAssembly(typeof(InventoryManagementPlatformDbContext).Assembly);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await base.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new ConcurrencyConflictException(
                    "The record was modified by another user. Please retry.");
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException { Number: 2601 or 2627 })
            {
                throw new UniqueConstraintViolationException(
                    "A record with this value already exists.");
            }
        }

    }
}
