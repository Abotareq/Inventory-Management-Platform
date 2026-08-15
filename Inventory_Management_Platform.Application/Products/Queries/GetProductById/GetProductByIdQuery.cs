using ErrorOr;
using Inventory_Management_Platform.Contracts.Product;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Products.Queries.GetProductById
{
    public sealed record GetProductByIdQuery(Guid ProductId) : IRequest<ErrorOr<ProductResponse>>;
}
