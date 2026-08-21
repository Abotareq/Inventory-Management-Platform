using Inventory_Management_Platform.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Common.Interfaces.Persistence
{
    public interface IAuditLogRepository
    {
        Task<(List<AuditLog> Items, int TotalCount)> GetPagedAsync(
            int pageNumber, int pageSize, string? entityName = null, string? entityId = null);
    }
}
