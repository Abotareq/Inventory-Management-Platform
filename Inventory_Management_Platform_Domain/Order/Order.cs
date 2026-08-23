using ErrorOr;
using Inventory_Management_Platform.Domain.Common.Models;
using Inventory_Management_Platform.Domain.DomainErrors;
using Inventory_Management_Platform.Domain.Order.Entites;
using Inventory_Management_Platform.Domain.Order.Enums;
using Inventory_Management_Platform.Domain.Order.Events;
using Inventory_Management_Platform.Domain.Order.ValueObjects;
using Inventory_Management_Platform.Domain.Product.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Domain.Order
{
    public sealed class Order : AggregateRoot
    {
        private readonly List<OrderItem> _items = new();

        public OrderId OrderId { get; private set; }
        public CustomerId CustomerId { get; private set; }
        public OrderStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public IReadOnlyList<OrderItem> Items => _items.AsReadOnly();
        public decimal TotalAmount => _items.Sum(i => i.LineTotal);

        private Order(OrderId orderId, CustomerId customerId, DateTime createdAt)
            : base(orderId.Value)
        {
            OrderId = orderId;
            CustomerId = customerId;
            Status = OrderStatus.Draft;
            CreatedAt = createdAt;
        }

        private Order() { }

        public static ErrorOr<Order> Create(CustomerId customerId)
        {
            return new Order(OrderId.CreateUnique(), customerId, DateTime.UtcNow);
        }

        public ErrorOr<Updated> AddItem(ProductId productId, int quantity, decimal unitPrice)
        {
            if (Status != OrderStatus.Draft)
                return Errors.Order.CannotModifyNonDraftOrder;

            if (quantity <= 0)
                return Errors.Order.InvalidQuantity;

            if (unitPrice < 0)
                return Errors.Order.InvalidUnitPrice;

            _items.Add(OrderItem.Create(OrderId, productId, quantity, unitPrice));

            return Result.Updated;
        }

        public ErrorOr<Updated> Submit(Guid performedByUserId)
        {
            if (Status != OrderStatus.Draft)
                return Errors.Order.InvalidStatusTransition;

            if (_items.Count == 0)
                return Errors.Order.EmptyOrder;

            var fromStatus = Status;
            Status = OrderStatus.Submitted;

            RaiseDomainEvent(new OrderCreated(OrderId, CustomerId, TotalAmount, performedByUserId, DateTime.UtcNow));
            RaiseDomainEvent(new OrderSubmitted(OrderId, fromStatus, performedByUserId, DateTime.UtcNow));

            return Result.Updated;
        }

        public ErrorOr<Updated> BeginProcessing(Guid performedByUserId)
        {
            if (Status != OrderStatus.Submitted)
                return Errors.Order.InvalidStatusTransition;

            var fromStatus = Status;
            Status = OrderStatus.Processing;

            RaiseDomainEvent(new OrderProcessingStarted(OrderId, fromStatus, performedByUserId, DateTime.UtcNow));

            return Result.Updated;
        }

        public ErrorOr<Updated> Complete(Guid performedByUserId)
        {
            if (Status != OrderStatus.Processing)
                return Errors.Order.InvalidStatusTransition;

            var fromStatus = Status;
            Status = OrderStatus.Completed;

            RaiseDomainEvent(new OrderCompleted(OrderId, fromStatus, performedByUserId, DateTime.UtcNow));

            return Result.Updated;
        }

        public ErrorOr<Updated> Cancel(Guid performedByUserId)
        {
            if (Status is OrderStatus.Completed or OrderStatus.Cancelled)
                return Errors.Order.InvalidStatusTransition;

            var fromStatus = Status;
            Status = OrderStatus.Cancelled;

            RaiseDomainEvent(new OrderCancelled(OrderId, fromStatus, performedByUserId, DateTime.UtcNow));

            return Result.Updated;
        }
    }
}
