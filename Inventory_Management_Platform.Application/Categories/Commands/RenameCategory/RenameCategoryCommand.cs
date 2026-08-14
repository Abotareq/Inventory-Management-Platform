using ErrorOr;
using Inventory_Management_Platform.Contracts.Category;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Categories.Commands.RenameCategory
{
    public sealed record RenameCategoryCommand(Guid CategoryId, string Name) : IRequest<ErrorOr<CategoryResponse>>;
}
