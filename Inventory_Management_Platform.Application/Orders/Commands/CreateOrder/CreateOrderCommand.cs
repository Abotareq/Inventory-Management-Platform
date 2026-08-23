using ErrorOr;
using Inventory_Management_Platform.Contracts.Order;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Orders.Commands.CreateOrder
{
    public sealed record CreateOrderItem(Guid ProductId, int Quantity);

    public sealed record CreateOrderCommand(
        Guid CustomerId,
        List<CreateOrderItem> Items,
        Guid PerformedByUserId) : IRequest<ErrorOr<OrderResponse>>;
}
