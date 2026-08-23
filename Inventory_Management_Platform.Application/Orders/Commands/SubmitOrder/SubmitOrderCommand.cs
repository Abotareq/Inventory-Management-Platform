using ErrorOr;
using Inventory_Management_Platform.Contracts.Order;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Orders.Commands.SubmitOrder
{
    public sealed record SubmitOrderCommand(
        Guid OrderId,
        Guid PerformedByUserId) : IRequest<ErrorOr<OrderResponse>>;
}
