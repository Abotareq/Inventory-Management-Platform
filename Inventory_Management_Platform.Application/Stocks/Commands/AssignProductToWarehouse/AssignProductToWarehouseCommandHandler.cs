using ErrorOr;
using Inventory_Management_Platform.Application.Common.Exceptions;
using Inventory_Management_Platform.Application.Common.Interfaces.Persistence;
using Inventory_Management_Platform.Contracts.Stock;
using Inventory_Management_Platform.Domain.DomainErrors;
using Inventory_Management_Platform.Domain.Product.ValueObjects;
using Inventory_Management_Platform.Domain.Warehouse.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using DomainStock = Inventory_Management_Platform.Domain.Stock.Stock;

namespace Inventory_Management_Platform.Application.Stocks.Commands.AssignProductToWarehouse
{
    public sealed class AssignProductToWarehouseCommandHandler
        : IRequestHandler<AssignProductToWarehouseCommand, ErrorOr<StockResponse>>
    {
        private readonly IStockRepository _stockRepository;
        private readonly IProductRepository _productRepository;
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AssignProductToWarehouseCommandHandler(
            IStockRepository stockRepository,
            IProductRepository productRepository,
            IWarehouseRepository warehouseRepository,
            IUnitOfWork unitOfWork)
        {
            _stockRepository = stockRepository;
            _productRepository = productRepository;
            _warehouseRepository = warehouseRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<StockResponse>> Handle(
      AssignProductToWarehouseCommand request, CancellationToken cancellationToken)
        {
            var productId = ProductId.Create(request.ProductId);
            var warehouseId = WarehouseId.Create(request.WarehouseId);

            var product = await _productRepository.GetByIdAsync(productId);
            if (product is null)
                return Errors.Product.NotFound;

            var warehouse = await _warehouseRepository.GetByIdAsync(warehouseId);
            if (warehouse is null)
                return Errors.Warehouse.NotFound;

            if (await _stockRepository.ExistsAsync(productId, warehouseId))
                return Errors.Stock.AlreadyExists;

            var stockResult = DomainStock.Create(productId, warehouseId);
            if (stockResult.IsError)
                return stockResult.Errors;

            var stock = stockResult.Value;

            try
            {
                await _stockRepository.AddAsync(stock);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
            catch (UniqueConstraintViolationException)
            {
                return Errors.Stock.AlreadyExists;
            }

            return  new StockResponse(
    stock.StockId.Value, stock.ProductId.Value, stock.WarehouseId.Value,
    stock.Quantity, stock.Reserved, stock.Available);
        }
    }
}
