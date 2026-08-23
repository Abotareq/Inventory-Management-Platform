using ErrorOr;
using Inventory_Management_Platform.Application.Common.Interfaces.Persistence;
using Inventory_Management_Platform.Contracts.Order;
using Inventory_Management_Platform.Domain.DomainErrors;
using Inventory_Management_Platform.Domain.Order;
using Inventory_Management_Platform.Domain.Order.ValueObjects;
using Inventory_Management_Platform.Domain.Product.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Orders.Commands.CreateOrder
{
    public sealed class CreateOrderCommandHandler
        : IRequestHandler<CreateOrderCommand, ErrorOr<OrderResponse>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateOrderCommandHandler(
            IOrderRepository orderRepository,
            IProductRepository productRepository,
            IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<OrderResponse>> Handle(
            CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var customerId = CustomerId.Create(request.CustomerId);

            var orderResult = Order.Create(customerId);
            if (orderResult.IsError)
                return orderResult.Errors;

            var order = orderResult.Value;

            foreach (var item in request.Items)
            {
                var productId = ProductId.Create(item.ProductId);

                var product = await _productRepository.GetByIdAsync(productId);
                if (product is null)
                    return Errors.Product.NotFound;

                var addItemResult = order.AddItem(productId, item.Quantity, product.Price);
                if (addItemResult.IsError)
                    return addItemResult.Errors;
            }

            await _orderRepository.AddAsync(order);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return MapToResponse(order);
        }

        private static OrderResponse MapToResponse(Order order)
        {
            var items = order.Items
                .Select(i => new OrderItemResponse(
                    i.OrderItemId.Value, i.ProductId.Value, i.Quantity, i.UnitPriceSnapshot, i.LineTotal))
                .ToList();

            return new OrderResponse(
                order.OrderId.Value, order.CustomerId.Value, order.Status.ToString(),
                order.CreatedAt, order.TotalAmount, items);
        }
    }
}
