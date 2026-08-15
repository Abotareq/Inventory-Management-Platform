using ErrorOr;
using Inventory_Management_Platform.Application.Products.Commands.CreateProduct;
using Inventory_Management_Platform.Application.Products.Commands.UpdateProduct;
using Inventory_Management_Platform.Application.Products.Queries.GetProductById;
using Inventory_Management_Platform.Application.Products.Queries.GetProducts;
using Inventory_Management_Platform.Contracts.Product;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory_Management_Platform.Api.Controllers
{
    [Route("api/products")]
    public sealed class ProductsController : ApiController
    {
        private readonly ISender _mediator;

        public ProductsController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create(CreateProductRequest request)
        {
            var command = new CreateProductCommand(
                request.Name, request.Sku, request.Description, request.CategoryId);

            ErrorOr<ProductResponse> result = await _mediator.Send(command);

            return result.Match(
                response => Ok(response),
                errors => Problem(errors));
        }
        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Update(Guid id, UpdateProductRequest request)
        {
            var command = new UpdateProductCommand(
                id, request.Name, request.Sku, request.Description, request.CategoryId);

            ErrorOr<ProductResponse> result = await _mediator.Send(command);

            return result.Match(
                response => Ok(response),
                errors => Problem(errors));
        }
        [HttpGet("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> GetById(Guid id)
        {
            var query = new GetProductByIdQuery(id);

            ErrorOr<ProductResponse> result = await _mediator.Send(query);

            return result.Match(
                response => Ok(response),
                errors => Problem(errors));
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
        {
            var query = new GetProductsQuery(pageNumber, pageSize);

            ErrorOr<PagedProductsResponse> result = await _mediator.Send(query);

            return result.Match(
                response => Ok(response),
                errors => Problem(errors));
        }
    }
}
