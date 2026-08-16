using Inventory_Management_Platform.Application.Common.Interfaces.Persistence;
using Inventory_Management_Platform.Domain.Stock.Entites;
using Inventory_Management_Platform.Domain.Stock.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Stocks.EventHandlers
{

    public sealed class StockDecreasedHandler : INotificationHandler<StockDecreased>
    {
        private readonly IStockRepository _stockRepository;

        public StockDecreasedHandler(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }

        public async Task Handle(StockDecreased notification, CancellationToken cancellationToken)
        {
            var adjustment = StockAdjustment.Create(
                notification.StockId,
                -notification.Delta,
                notification.ResultingQuantity,
                notification.Reason,
                notification.PerformedByUserId,
                notification.Timestamp);

            await _stockRepository.AddAdjustmentAsync(notification.StockId, adjustment);
        }
    }
}
