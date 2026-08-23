using ErrorOr;
using Inventory_Management_Platform.Application.Common.Interfaces.Persistence;
using Inventory_Management_Platform.Application.Orders.Common;
using Inventory_Management_Platform.Contracts.Order;
using Inventory_Management_Platform.Domain.Order.Enums;
using Inventory_Management_Platform.Domain.Order.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Orders.Queries.GetOrders
{
    public sealed class GetOrdersQueryHandler
        : IRequestHandler<GetOrdersQuery, ErrorOr<PagedOrdersResponse>>
    {
        private readonly IOrderRepository _orderRepository;

        public GetOrdersQueryHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<ErrorOr<PagedOrdersResponse>> Handle(
            GetOrdersQuery request, CancellationToken cancellationToken)
        {
            CustomerId? customerId = request.CustomerId is not null
                ? CustomerId.Create(request.CustomerId.Value)
                : null;

            OrderStatus? status = request.Status is not null
                ? Enum.Parse<OrderStatus>(request.Status, ignoreCase: true)
                : null;

            var (items, totalCount) = await _orderRepository.GetPagedAsync(
                request.PageNumber, request.PageSize, customerId, status, request.FromDate, request.ToDate);

            var response = items.Select(o => o.ToResponse()).ToList();

            return new PagedOrdersResponse(response, request.PageNumber, request.PageSize, totalCount);
        }
    }
}
