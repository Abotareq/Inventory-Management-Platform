using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Contracts.Audit
{
    public sealed record PagedAuditLogsResponse(
       List<AuditLogResponse> Items,
       int PageNumber,
       int PageSize,
       int TotalCount);
}
