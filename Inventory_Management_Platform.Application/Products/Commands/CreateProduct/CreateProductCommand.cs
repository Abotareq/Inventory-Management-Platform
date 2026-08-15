using ErrorOr;
using Inventory_Management_Platform.Contracts.Product;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Products.Commands.CreateProduct
{
    public sealed record CreateProductCommand(
         string Name,
         string Sku,
         string? Description,
         Guid? CategoryId) : IRequest<ErrorOr<ProductResponse>>;
}
