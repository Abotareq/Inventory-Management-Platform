using Inventory_Management_Platform.Domain.Common.Interfaces;
using Inventory_Management_Platform.Domain.Stock.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Domain.Stock.Events
{
    public sealed record StockIncreased(
       StockId StockId,
       int Delta,
       int ResultingQuantity,
       string Reason,
       Guid PerformedByUserId,
       DateTime Timestamp) : IDomainEvent;

    public sealed record StockDecreased(
        StockId StockId,
        int Delta,
        int ResultingQuantity,
        string Reason,
        Guid PerformedByUserId,
        DateTime Timestamp) : IDomainEvent;
}
