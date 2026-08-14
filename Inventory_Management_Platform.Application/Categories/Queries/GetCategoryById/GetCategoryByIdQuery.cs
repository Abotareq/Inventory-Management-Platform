using ErrorOr;
using Inventory_Management_Platform.Contracts.Category;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Categories.Queries.GetCategoryById
{
    public sealed record GetCategoryByIdQuery(Guid CategoryId) : IRequest<ErrorOr<CategoryResponse>>;
}
