using ErrorOr;
using Inventory_Management_Platform.Application.Warehouses.Commands.CreateWarehouse;
using Inventory_Management_Platform.Application.Warehouses.Commands.UpdateWarehouse;
using Inventory_Management_Platform.Application.Warehouses.Queries.GetWarehouseById;
using Inventory_Management_Platform.Application.Warehouses.Queries.GetWarehouses;
using Inventory_Management_Platform.Contracts.Warehouse;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory_Management_Platform.Api.Controllers
{
    [Route("api/warehouses")]
    public sealed class WarehousesController : ApiController
    {
        private readonly ISender _mediator;

        public WarehousesController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create(CreateWarehouseRequest request)
        {
            var command = new CreateWarehouseCommand(request.Name, request.Location);

            ErrorOr<WarehouseResponse> result = await _mediator.Send(command);

            return result.Match(
                response => Ok(response),
                errors => Problem(errors));
        }
        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Update(Guid id, UpdateWarehouseRequest request)
        {
            var command = new UpdateWarehouseCommand(id, request.Name, request.Location);

            ErrorOr<WarehouseResponse> result = await _mediator.Send(command);

            return result.Match(
                response => Ok(response),
                errors => Problem(errors));
        }
        [HttpGet("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> GetById(Guid id)
        {
            var query = new GetWarehouseByIdQuery(id);

            ErrorOr<WarehouseResponse> result = await _mediator.Send(query);

            return result.Match(
                response => Ok(response),
                errors => Problem(errors));
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetWarehousesQuery();

            ErrorOr<List<WarehouseResponse>> result = await _mediator.Send(query);

            return result.Match(
                response => Ok(response),
                errors => Problem(errors));
        }
    }
}
