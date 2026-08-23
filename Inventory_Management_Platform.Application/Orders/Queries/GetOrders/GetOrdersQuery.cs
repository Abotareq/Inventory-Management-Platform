using ErrorOr;
using Inventory_Management_Platform.Contracts.Order;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Orders.Queries.GetOrders
{
    public sealed record GetOrdersQuery(
        int PageNumber,
        int PageSize,
        Guid? CustomerId,
        string? Status,
        DateTime? FromDate,
        DateTime? ToDate) : IRequest<ErrorOr<PagedOrdersResponse>>;
}
