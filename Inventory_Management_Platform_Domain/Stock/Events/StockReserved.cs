using Inventory_Management_Platform.Domain.Common.Interfaces;
using Inventory_Management_Platform.Domain.Order.ValueObjects;
using Inventory_Management_Platform.Domain.Stock.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Domain.Stock.Events
{
    public sealed record StockReserved(
        StockId StockId, int Amount, int ResultingReserved,
        OrderId OrderId, Guid PerformedByUserId, DateTime Timestamp) : IDomainEvent;
}
