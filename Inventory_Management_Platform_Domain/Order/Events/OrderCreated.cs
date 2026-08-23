using Inventory_Management_Platform.Domain.Common.Interfaces;
using Inventory_Management_Platform.Domain.Order.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Domain.Order.Events
{
    public sealed record OrderCreated(
        OrderId OrderId, CustomerId CustomerId, decimal TotalAmount,
        Guid PerformedByUserId, DateTime Timestamp) : IDomainEvent;
}
