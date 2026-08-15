using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Categories.Commands.DeleteCategory
{
    public sealed record DeleteCategoryCommand(Guid CategoryId) : IRequest<ErrorOr<Deleted>>;

}
