using ErrorOr;
using Inventory_Management_Platform.Application.Common.Interfaces.Persistence;
using Inventory_Management_Platform.Application.Orders.Common;
using Inventory_Management_Platform.Contracts.Order;
using Inventory_Management_Platform.Domain.DomainErrors;
using Inventory_Management_Platform.Domain.Order;
using Inventory_Management_Platform.Domain.Order.ValueObjects;
using Inventory_Management_Platform.Domain.Product.ValueObjects;
using Inventory_Management_Platform.Domain.Warehouse.ValueObjects;
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
        private readonly IStockRepository _stockRepository;
        public CreateOrderCommandHandler(
            IOrderRepository orderRepository,
            IProductRepository productRepository,
            IUnitOfWork unitOfWork,
            IStockRepository stockRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _stockRepository = stockRepository;
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
                var warehouseId = WarehouseId.Create(item.WarehouseId);

                var product = await _productRepository.GetByIdAsync(productId);
                if (product is null)
                    return Errors.Product.NotFound;

                var stockExists = await _stockRepository.ExistsAsync(productId, warehouseId);
                if (!stockExists)
                    return Errors.Stock.NotFound;

                var addItemResult = order.AddItem(productId, warehouseId, item.Quantity, product.Price);
                if (addItemResult.IsError)
                    return addItemResult.Errors;
            }

            await _orderRepository.AddAsync(order);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return order.ToResponse();
        }

      
    }
}
