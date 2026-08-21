using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Contracts.Audit
{
    public sealed record AuditLogResponse(
         Guid Id,
         string EntityName,
         string EntityId,
         string Action,
         string? Changes,
         Guid? PerformedByUserId,
         DateTime Timestamp);
}
