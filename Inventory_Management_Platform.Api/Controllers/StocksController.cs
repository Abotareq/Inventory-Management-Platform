using ErrorOr;
using Inventory_Management_Platform.Application.Stocks.Commands.AssignProductToWarehouse;
using Inventory_Management_Platform.Contracts.Stock;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory_Management_Platform.Api.Controllers
{
    [Route("api/stocks")]
    public sealed class StocksController : ApiController
    {
        private readonly ISender _mediator;

        public StocksController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("assign")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> AssignProductToWarehouse(AssignProductToWarehouseRequest request)
        {
            var command = new AssignProductToWarehouseCommand(request.ProductId, request.WarehouseId);

            ErrorOr<StockResponse> result = await _mediator.Send(command);

            return result.Match(
                response => Ok(response),
                errors => Problem(errors));
        }
    }
}
