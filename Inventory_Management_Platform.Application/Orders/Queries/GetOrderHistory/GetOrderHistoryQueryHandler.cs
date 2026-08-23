using ErrorOr;
using Inventory_Management_Platform.Application.Common.Interfaces.Persistence;
using Inventory_Management_Platform.Contracts.Order;
using Inventory_Management_Platform.Domain.DomainErrors;
using Inventory_Management_Platform.Domain.Order.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Orders.Queries.GetOrderHistory
{
    public sealed class GetOrderHistoryQueryHandler
           : IRequestHandler<GetOrderHistoryQuery, ErrorOr<PagedOrderHistoryResponse>>
    {
        private readonly IOrderRepository _orderRepository;

        public GetOrderHistoryQueryHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<ErrorOr<PagedOrderHistoryResponse>> Handle(
            GetOrderHistoryQuery request, CancellationToken cancellationToken)
        {
            var orderId = OrderId.Create(request.OrderId);

            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order is null)
                return Errors.Order.NotFound;

            var (items, totalCount) = await _orderRepository.GetHistoryAsync(
                orderId, request.PageNumber, request.PageSize);

            var response = items
                .Select(h => new OrderHistoryResponse(
                    h.OrderHistoryId.Value, h.OrderId.Value, h.FromStatus.ToString(),
                    h.ToStatus.ToString(), h.PerformedByUserId, h.Timestamp))
                .ToList();

            return new PagedOrderHistoryResponse(response, request.PageNumber, request.PageSize, totalCount);
        }
    }
}
