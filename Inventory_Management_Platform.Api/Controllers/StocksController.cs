using ErrorOr;
using Inventory_Management_Platform.Application.Stocks.Commands.AdjustStock;
using Inventory_Management_Platform.Application.Stocks.Commands.AssignProductToWarehouse;
using Inventory_Management_Platform.Application.Stocks.Queries.GetStockByWarehouse;
using Inventory_Management_Platform.Contracts.Stock;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        [HttpPost("adjust")]
        [Authorize(Roles = "WarehouseOperator")]
        public async Task<IActionResult> AdjustStock(AdjustStockRequest request)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var command = new AdjustStockCommand(
                request.ProductId, request.WarehouseId, request.Amount, request.Reason, userId);

            ErrorOr<StockResponse> result = await _mediator.Send(command);

            return result.Match(
                response => Ok(response),
                errors => Problem(errors));
        }
        [HttpGet("warehouse/{warehouseId:guid}")]
        [Authorize]
        public async Task<IActionResult> GetByWarehouse(
    Guid warehouseId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
        {
            var query = new GetStockByWarehouseQuery(warehouseId, pageNumber, pageSize);

            ErrorOr<PagedStockResponse> result = await _mediator.Send(query);

            return result.Match(
                response => Ok(response),
                errors => Problem(errors));
        }
    }
}
