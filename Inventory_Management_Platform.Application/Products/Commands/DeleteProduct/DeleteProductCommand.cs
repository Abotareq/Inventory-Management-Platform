using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Products.Commands.DeleteProduct
{
    public sealed record DeleteProductCommand(Guid ProductId) : IRequest<ErrorOr<Deleted>>;
}
