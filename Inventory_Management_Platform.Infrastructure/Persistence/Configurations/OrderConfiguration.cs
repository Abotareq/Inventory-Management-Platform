using Inventory_Management_Platform.Domain.Order.Entites;
using Inventory_Management_Platform.Domain.Order.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using DomainOrder = Inventory_Management_Platform.Domain.Order.Order;

namespace Inventory_Management_Platform.Infrastructure.Persistence.Configurations
{
    public sealed class OrderConfiguration : IEntityTypeConfiguration<DomainOrder>
    {
        public void Configure(EntityTypeBuilder<DomainOrder> builder)
        {
            builder.ToTable("Orders");

            builder.HasKey(o => o.Id);

            builder.Property(o => o.OrderId)
                .HasConversion(id => id.Value, value => OrderId.Create(value))
                .HasColumnName("OrderId")
                .ValueGeneratedNever();

            builder.Property(o => o.CustomerId)
                .HasConversion(id => id.Value, value => CustomerId.Create(value))
                .HasColumnName("CustomerId");

            builder.Property(o => o.Status)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(o => o.CreatedAt)
                .IsRequired();

            builder.Property(o => o.RowVersion)
                .IsRowVersion();

            builder.Ignore(o => o.TotalAmount);
            builder.Ignore(o => o.Items);

            builder.HasMany<OrderItem>("_items")
                .WithOne()
                .HasForeignKey(i => i.OrderId)
                .HasPrincipalKey(o => o.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
