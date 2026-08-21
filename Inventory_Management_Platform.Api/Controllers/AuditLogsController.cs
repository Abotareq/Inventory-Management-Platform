using ErrorOr;
using Inventory_Management_Platform.Application.AuditLogs.Queries;
using Inventory_Management_Platform.Contracts.Audit;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Api.Controllers
{

    [Route("api/audit-logs")]
    public sealed class AuditLogsController : ApiController
    {
        private readonly ISender _mediator;

        public AuditLogsController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> GetAll(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string? entityName = null,
            [FromQuery] string? entityId = null)
        {
            var query = new GetAuditLogsQuery(pageNumber, pageSize, entityName, entityId);

            ErrorOr<PagedAuditLogsResponse> result = await _mediator.Send(query);

            return result.Match(
                response => Ok(response),
                errors => Problem(errors));
        }
    }
}
