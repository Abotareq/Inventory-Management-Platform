using ErrorOr;
using Inventory_Management_Platform.Application.Orders.Commands.BeginProcessing;
using Inventory_Management_Platform.Application.Orders.Commands.CancelOrder;
using Inventory_Management_Platform.Application.Orders.Commands.CompleteOrder;
using Inventory_Management_Platform.Application.Orders.Commands.CreateOrder;
using Inventory_Management_Platform.Application.Orders.Commands.SubmitOrder;
using Inventory_Management_Platform.Application.Orders.Queries.GetOrderById;
using Inventory_Management_Platform.Contracts.Order;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Inventory_Management_Platform.Api.Controllers
{
    [Route("api/orders")]
    public sealed class OrdersController : ApiController
    {
        private readonly ISender _mediator;

        public OrdersController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "SalesAgent")]
        public async Task<IActionResult> Create(CreateOrderRequest request)
        {
            var userId = Guid.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Sub)!);

            var items = request.Items
                .Select(i => new CreateOrderItem(i.ProductId, i.WarehouseId, i.Quantity))
                .ToList();

            var command = new CreateOrderCommand(request.CustomerId, items, userId);

            ErrorOr<OrderResponse> result = await _mediator.Send(command);

            return result.Match(
                response => Ok(response),
                errors => Problem(errors));
        }
        [HttpPost("{id:guid}/submit")]
        [Authorize(Roles = "SalesAgent")]
        public async Task<IActionResult> Submit(Guid id)
        {
            var userId = Guid.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Sub)!);

            var command = new SubmitOrderCommand(id, userId);

            ErrorOr<OrderResponse> result = await _mediator.Send(command);

            return result.Match(
                response => Ok(response),
                errors => Problem(errors));
        }
        [HttpPost("{id:guid}/begin-processing")]
        [Authorize(Roles = "WarehouseOperator")]
        public async Task<IActionResult> BeginProcessing(Guid id)
        {
            var userId = Guid.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Sub)!);

            var command = new BeginProcessingCommand(id, userId);

            ErrorOr<OrderResponse> result = await _mediator.Send(command);

            return result.Match(
                response => Ok(response),
                errors => Problem(errors));
        }
        [HttpPost("{id:guid}/complete")]
        [Authorize(Roles = "WarehouseOperator")]
        public async Task<IActionResult> Complete(Guid id)
        {
            var userId = Guid.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Sub)!);

            var command = new CompleteOrderCommand(id, userId);

            ErrorOr<OrderResponse> result = await _mediator.Send(command);

            return result.Match(
                response => Ok(response),
                errors => Problem(errors));
        }
        [HttpPost("{id:guid}/cancel")]
        [Authorize(Roles = "SalesAgent,WarehouseOperator")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            var userId = Guid.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Sub)!);

            var command = new CancelOrderCommand(id, userId);

            ErrorOr<OrderResponse> result = await _mediator.Send(command);

            return result.Match(
                response => Ok(response),
                errors => Problem(errors));
        }
        [HttpGet("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> GetById(Guid id)
        {
            var query = new GetOrderByIdQuery(id);

            ErrorOr<OrderResponse> result = await _mediator.Send(query);

            return result.Match(
                response => Ok(response),
                errors => Problem(errors));
        }
    }
}
