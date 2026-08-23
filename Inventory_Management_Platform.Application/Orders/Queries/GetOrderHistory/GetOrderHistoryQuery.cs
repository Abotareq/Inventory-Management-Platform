using ErrorOr;
using Inventory_Management_Platform.Contracts.Order;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Orders.Queries.GetOrderHistory
{
    public sealed record GetOrderHistoryQuery(
         Guid OrderId,
         int PageNumber,
         int PageSize) : IRequest<ErrorOr<PagedOrderHistoryResponse>>;
}
