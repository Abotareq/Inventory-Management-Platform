using Inventory_Management_Platform.Application.Common.Interfaces.Persistence;
using Inventory_Management_Platform.Domain.Stock.Entites;
using Inventory_Management_Platform.Domain.Stock.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Stocks.EventHandlers
{
   
        public sealed class StockReservedHandler : INotificationHandler<StockReserved>
        {
            private readonly IStockRepository _stockRepository;

            public StockReservedHandler(IStockRepository stockRepository)
            {
                _stockRepository = stockRepository;
            }

            public async Task Handle(StockReserved notification, CancellationToken cancellationToken)
            {
                var reservation = StockReservation.Create(
                    notification.StockId, notification.OrderId, notification.Amount, "Reserved",
                    notification.PerformedByUserId, notification.Timestamp);

                await _stockRepository.AddReservationAsync(reservation);
            }
        }

 }
