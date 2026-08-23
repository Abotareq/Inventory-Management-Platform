using Inventory_Management_Platform.Domain.Common.Interfaces;
using Inventory_Management_Platform.Domain.Order.Enums;
using Inventory_Management_Platform.Domain.Order.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Domain.Order.Events
{
    public sealed record OrderCancelled(
       OrderId OrderId, OrderStatus PreviousStatus, Guid PerformedByUserId, DateTime Timestamp) : IDomainEvent;
}
