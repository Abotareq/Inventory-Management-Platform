using ErrorOr;
using Inventory_Management_Platform.Contracts.Audit;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.AuditLogs.Queries
{
    public sealed record GetAuditLogsQuery(
      int PageNumber,
      int PageSize,
      string? EntityName,
      string? EntityId) : IRequest<ErrorOr<PagedAuditLogsResponse>>;
}
