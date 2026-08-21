using System;
using System.Collections.Generic;
using System.Text;
using Inventory_Management_Platform.Application.Common.Interfaces.Persistence;
using Inventory_Management_Platform.Application.Common.Models;
using Microsoft.EntityFrameworkCore;
namespace Inventory_Management_Platform.Infrastructure.Persistence.Auditing
{
    public sealed class AuditLogRepository : IAuditLogRepository
    {
        private readonly InventoryManagementPlatformDbContext _dbContext;

        public AuditLogRepository(InventoryManagementPlatformDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<(List<AuditLog> Items, int TotalCount)> GetPagedAsync(
            int pageNumber, int pageSize, string? entityName = null, string? entityId = null)
        {
            var query = _dbContext.AuditLogs.AsQueryable();

            if (!string.IsNullOrWhiteSpace(entityName))
                query = query.Where(a => a.EntityName == entityName);

            if (!string.IsNullOrWhiteSpace(entityId))
                query = query.Where(a => a.EntityId == entityId);

            query = query.OrderByDescending(a => a.Timestamp);

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }
    }
}
