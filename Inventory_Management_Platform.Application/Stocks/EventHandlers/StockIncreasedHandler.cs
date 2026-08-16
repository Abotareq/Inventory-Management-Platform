using Inventory_Management_Platform.Application.Common.Interfaces.Persistence;
using Inventory_Management_Platform.Domain.Stock.Entites;
using Inventory_Management_Platform.Domain.Stock.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Stocks.EventHandlers
{
    public sealed class StockIncreasedHandler : INotificationHandler<StockIncreased>
    {
        private readonly IStockRepository _stockRepository;

        public StockIncreasedHandler(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }

        public async Task Handle(StockIncreased notification, CancellationToken cancellationToken)
        {
            var adjustment = StockAdjustment.Create(
                notification.StockId,
                notification.Delta,
                notification.ResultingQuantity,
                notification.Reason,
                notification.PerformedByUserId,
                notification.Timestamp);

            await _stockRepository.AddAdjustmentAsync(notification.StockId, adjustment);
        }
    }
}
