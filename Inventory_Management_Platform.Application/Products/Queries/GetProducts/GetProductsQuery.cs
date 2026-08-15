using ErrorOr;
using Inventory_Management_Platform.Contracts.Product;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Products.Queries.GetProducts
{
    public sealed record GetProductsQuery(int PageNumber, int PageSize)
      : IRequest<ErrorOr<PagedProductsResponse>>;
}
