using Inventory_Management_Platform.Application.Common.Interfaces.Persistence;
using Inventory_Management_Platform.Domain.Stock.Entites;
using Inventory_Management_Platform.Domain.Stock.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Stocks.EventHandlers
{
    public sealed class StockReleasedHandler : INotificationHandler<StockReleased>
    {
        private readonly IStockRepository _stockRepository;

        public StockReleasedHandler(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }

        public async Task Handle(StockReleased notification, CancellationToken cancellationToken)
        {
            var reservation = StockReservation.Create(
                notification.StockId, notification.OrderId, notification.Amount, "Released",
                notification.PerformedByUserId, notification.Timestamp);

            await _stockRepository.AddReservationAsync(reservation);
        }
    }
}
