using ErrorOr;
using Inventory_Management_Platform.Contracts.Product;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Products.Commands.UpdateProduct
{
    public sealed record UpdateProductCommand(
        Guid ProductId,
        string Name,
        string Sku,
        string? Description,
        Guid? CategoryId,
         decimal Price) : IRequest<ErrorOr<ProductResponse>>;
}
