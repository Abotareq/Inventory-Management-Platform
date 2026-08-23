using ErrorOr;
using Inventory_Management_Platform.Contracts.Order;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Orders.Commands.BeginProcessing
{
    public sealed record BeginProcessingCommand(
          Guid OrderId,
          Guid PerformedByUserId) : IRequest<ErrorOr<OrderResponse>>;
}
