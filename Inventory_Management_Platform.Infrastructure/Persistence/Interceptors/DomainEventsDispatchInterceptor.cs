using Inventory_Management_Platform.Domain.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Infrastructure.Persistence.Interceptors
{
    public sealed class DomainEventsDispatchInterceptor : SaveChangesInterceptor
    {
        private readonly IPublisher _publisher;

        public DomainEventsDispatchInterceptor(IPublisher publisher)
        {
            _publisher = publisher;
        }

        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            var context = eventData.Context;
            if (context is null)
                return await base.SavingChangesAsync(eventData, result, cancellationToken);

            var aggregatesWithEvents = context.ChangeTracker
                .Entries<AggregateRoot>()
                .Select(e => e.Entity)
                .Where(a => a.DomainEvents.Any())
                .ToList();

            var domainEvents = aggregatesWithEvents
                .SelectMany(a => a.DomainEvents)
                .ToList();

            foreach (var aggregate in aggregatesWithEvents)
            {
                aggregate.ClearDomainEvents();
            }

            foreach (var domainEvent in domainEvents)
            {
                await _publisher.Publish(domainEvent, cancellationToken);
            }

            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}
