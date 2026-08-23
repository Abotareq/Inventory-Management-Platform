using Inventory_Management_Platform.Domain.Order;
using Inventory_Management_Platform.Domain.Order.Enums;
using Inventory_Management_Platform.Domain.Order.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Application.Common.Interfaces.Persistence
{
    public interface IOrderRepository
    {
        Task<Order?> GetByIdAsync(OrderId id);
        Task<Order?> GetByIdWithItemsAsync(OrderId id);
        Task<(List<Order> Items, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            CustomerId? customerId = null,
            OrderStatus? status = null,
            DateTime? fromDate = null,
            DateTime? toDate = null);
        Task AddAsync(Order order);
        void Update(Order order);
    }
}
