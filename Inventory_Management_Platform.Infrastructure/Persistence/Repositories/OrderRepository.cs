using Inventory_Management_Platform.Application.Common.Interfaces.Persistence;
using Inventory_Management_Platform.Domain.Order.Entites;
using Inventory_Management_Platform.Domain.Order.Enums;
using Inventory_Management_Platform.Domain.Order.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using DomainOrder = Inventory_Management_Platform.Domain.Order.Order;
namespace Inventory_Management_Platform.Infrastructure.Persistence.Repositories
{
    public sealed class OrderRepository : IOrderRepository
    {
        private readonly InventoryManagementPlatformDbContext _dbContext;

        public OrderRepository(InventoryManagementPlatformDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<DomainOrder?> GetByIdAsync(OrderId id)
        {
            return await _dbContext.Orders
                .FirstOrDefaultAsync(o => o.OrderId == id);
        }

        public async Task<DomainOrder?> GetByIdWithItemsAsync(OrderId id)
        {
            return await _dbContext.Orders
                .Include("_items")
                .FirstOrDefaultAsync(o => o.OrderId == id);
        }

        public async Task<(List<DomainOrder> Items, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            CustomerId? customerId = null,
            OrderStatus? status = null,
            DateTime? fromDate = null,
            DateTime? toDate = null)
        {
            var query = _dbContext.Orders.AsQueryable();

            if (customerId is not null)
                query = query.Where(o => o.CustomerId == customerId);

            if (status is not null)
                query = query.Where(o => o.Status == status);

            if (fromDate is not null)
                query = query.Where(o => o.CreatedAt >= fromDate);

            if (toDate is not null)
                query = query.Where(o => o.CreatedAt <= toDate);

            query = query.OrderByDescending(o => o.CreatedAt);

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task AddAsync(DomainOrder order)
        {
            await _dbContext.Orders.AddAsync(order);
        }

        public void Update(DomainOrder order)
        {
            _dbContext.Orders.Update(order);
        }

        public async Task AddHistoryAsync(OrderHistory history)
        {
            await _dbContext.Set<OrderHistory>().AddAsync(history);
        }

        public async Task<(List<OrderHistory> Items, int TotalCount)> GetHistoryAsync(
            OrderId orderId, int pageNumber, int pageSize)
        {
            var query = _dbContext.Set<OrderHistory>()
                .Where(h => h.OrderId == orderId)
                .OrderByDescending(h => h.Timestamp);

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }
    }
}
