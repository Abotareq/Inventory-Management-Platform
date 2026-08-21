using Inventory_Management_Platform.Application.Common.Interfaces.Services;
using Inventory_Management_Platform.Application.Common.Models;
using Inventory_Management_Platform.Domain.Common.Models;
using Inventory_Management_Platform.Infrastructure.Persistence.Auditing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Inventory_Management_Platform.Infrastructure.Persistence.Interceptors
{
    public sealed class AuditInterceptor : SaveChangesInterceptor
    {
        private readonly ICurrentUserService _currentUserService;

        public AuditInterceptor(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            var context = eventData.Context;
            if (context is null)
                return base.SavingChangesAsync(eventData, result, cancellationToken);

            var auditEntries = new List<AuditLog>();
            var userId = _currentUserService.UserId;

            foreach (var entry in context.ChangeTracker.Entries<AggregateRoot>())
            {
                if (entry.State is EntityState.Unchanged or EntityState.Detached)
                    continue;

                var auditLog = new AuditLog
                {
                    Id = Guid.NewGuid(),
                    EntityName = entry.Entity.GetType().Name,
                    EntityId = entry.Property(nameof(AggregateRoot.Id)).CurrentValue?.ToString() ?? string.Empty,
                    Action = entry.State switch
                    {
                        EntityState.Added => "Created",
                        EntityState.Modified => "Modified",
                        EntityState.Deleted => "Deleted",
                        _ => "Unknown"
                    },
                    Changes = BuildChangesJson(entry),
                    PerformedByUserId = userId,
                    Timestamp = DateTime.UtcNow
                };

                auditEntries.Add(auditLog);
            }

            if (auditEntries.Count > 0)
            {
                context.Set<AuditLog>().AddRange(auditEntries);
            }

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private static string? BuildChangesJson(EntityEntry entry)
        {
            var changes = new Dictionary<string, object?>();

            foreach (var property in entry.Properties)
            {
                if (property.Metadata.Name == nameof(AggregateRoot.Id))
                    continue;

                switch (entry.State)
                {
                    case EntityState.Added:
                        changes[property.Metadata.Name] = property.CurrentValue;
                        break;
                    case EntityState.Deleted:
                        changes[property.Metadata.Name] = property.OriginalValue;
                        break;
                    case EntityState.Modified when property.IsModified:
                        changes[property.Metadata.Name] = new
                        {
                            old = property.OriginalValue,
                            @new = property.CurrentValue
                        };
                        break;
                }
            }

            return changes.Count > 0 ? JsonSerializer.Serialize(changes) : null;
        }
    }
}
