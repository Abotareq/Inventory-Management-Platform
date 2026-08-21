using ErrorOr;
using Inventory_Management_Platform.Application.Common.Interfaces.Persistence;
using Inventory_Management_Platform.Contracts.Audit;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.AuditLogs.Queries
{
    public sealed class GetAuditLogsQueryHandler
         : IRequestHandler<GetAuditLogsQuery, ErrorOr<PagedAuditLogsResponse>>
    {
        private readonly IAuditLogRepository _auditLogRepository;

        public GetAuditLogsQueryHandler(IAuditLogRepository auditLogRepository)
        {
            _auditLogRepository = auditLogRepository;
        }

        public async Task<ErrorOr<PagedAuditLogsResponse>> Handle(
            GetAuditLogsQuery request, CancellationToken cancellationToken)
        {
            var (items, totalCount) = await _auditLogRepository.GetPagedAsync(
                request.PageNumber, request.PageSize, request.EntityName, request.EntityId);

            var response = items
                .Select(a => new AuditLogResponse(
                    a.Id, a.EntityName, a.EntityId, a.Action, a.Changes, a.PerformedByUserId, a.Timestamp))
                .ToList();

            return new PagedAuditLogsResponse(response, request.PageNumber, request.PageSize, totalCount);
        }
    }
}
