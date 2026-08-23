using ErrorOr;
using Inventory_Management_Platform.Application.Common.Interfaces.Persistence;
using Inventory_Management_Platform.Contracts.Order;
using Inventory_Management_Platform.Domain.DomainErrors;
using Inventory_Management_Platform.Domain.Order.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Orders.Queries.GetOrderById
{
    public sealed class GetOrderByIdQueryHandler
         : IRequestHandler<GetOrderByIdQuery, ErrorOr<OrderResponse>>
    {
        private readonly IOrderRepository _orderRepository;

        public GetOrderByIdQueryHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<ErrorOr<OrderResponse>> Handle(
            GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdWithItemsAsync(OrderId.Create(request.OrderId));
            if (order is null)
                return Errors.Order.NotFound;

            return order.ToResponse();
        }
    }
}
