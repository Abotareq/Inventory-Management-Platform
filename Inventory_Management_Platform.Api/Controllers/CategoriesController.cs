using ErrorOr;
using Inventory_Management_Platform.Application.Categories.Commands.CreateCategory;
using Inventory_Management_Platform.Application.Categories.Commands.RenameCategory;
using Inventory_Management_Platform.Application.Categories.Queries.GetCategoryById;
using Inventory_Management_Platform.Contracts.Category;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory_Management_Platform.Api.Controllers
{
    [Route("api/categories")]
    public sealed class CategoriesController : ApiController
    {
        private readonly ISender _mediator;

        public CategoriesController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create(CreateCategoryRequest request)
        {
            var command = new CreateCategoryCommand(request.Name);

            ErrorOr<CategoryResponse> result = await _mediator.Send(command);

            return result.Match(
                response => Ok(response),
                errors => Problem(errors));
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Rename(Guid id, RenameCategoryRequest request)
        {
            var command = new RenameCategoryCommand(id, request.Name);

            ErrorOr<CategoryResponse> result = await _mediator.Send(command);

            return result.Match(
                response => Ok(response),
                errors => Problem(errors));
        }
        [HttpGet("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> GetById(Guid id)
        {
            var query = new GetCategoryByIdQuery(id);

            ErrorOr<CategoryResponse> result = await _mediator.Send(query);

            return result.Match(
                response => Ok(response),
                errors => Problem(errors));
        }
    }
}
