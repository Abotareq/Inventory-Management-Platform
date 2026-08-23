using ErrorOr;
using Inventory_Management_Platform.Application.Common.Interfaces.Services;
using Inventory_Management_Platform.Contracts.Order;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Orders.Commands.CreateOrder
{
    public sealed record CreateOrderItem(Guid ProductId, Guid WarehouseId, int Quantity);

    public sealed record CreateOrderCommand(
        Guid CustomerId,
        List<CreateOrderItem> Items,
        Guid PerformedByUserId,
        string IdempotencyKey) : IRequest<ErrorOr<OrderResponse>>, IIdempotentRequest;
}
