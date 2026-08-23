using Inventory_Management_Platform.Domain.Order.Entites;
using Inventory_Management_Platform.Domain.Order.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Infrastructure.Persistence.Configurations
{
    public sealed class OrderHistoryConfiguration : IEntityTypeConfiguration<OrderHistory>
    {
        public void Configure(EntityTypeBuilder<OrderHistory> builder)
        {
            builder.ToTable("OrderHistories");

            builder.HasKey(h => h.Id);

            builder.Property(h => h.OrderHistoryId)
                .HasConversion(id => id.Value, value => OrderHistoryId.Create(value))
                .HasColumnName("OrderHistoryId")
                .ValueGeneratedNever();

            builder.Property(h => h.OrderId)
                .HasConversion(id => id.Value, value => OrderId.Create(value))
                .HasColumnName("OrderId");

            builder.Property(h => h.FromStatus)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(h => h.ToStatus)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(h => h.PerformedByUserId)
                .IsRequired();

            builder.Property(h => h.Timestamp)
                .IsRequired();

            builder.HasIndex(h => h.OrderId);
        }
    }
}
