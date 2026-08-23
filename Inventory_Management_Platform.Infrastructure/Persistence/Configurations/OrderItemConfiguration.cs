using Inventory_Management_Platform.Domain.Order.Entites;
using Inventory_Management_Platform.Domain.Order.ValueObjects;
using Inventory_Management_Platform.Domain.Product;
using Inventory_Management_Platform.Domain.Product.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Infrastructure.Persistence.Configurations
{
    public sealed class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("OrderItems");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.OrderItemId)
                .HasConversion(id => id.Value, value => OrderItemId.Create(value))
                .HasColumnName("OrderItemId")
                .ValueGeneratedNever();

            builder.Property(i => i.OrderId)
                .HasConversion(id => id.Value, value => OrderId.Create(value))
                .HasColumnName("OrderId");

            builder.Property(i => i.ProductId)
                .HasConversion(id => id.Value, value => ProductId.Create(value))
                .HasColumnName("ProductId");

            builder.Property(i => i.Quantity)
                .IsRequired();

            builder.Property(i => i.UnitPriceSnapshot)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Ignore(i => i.LineTotal); // computed, not persisted

            builder.HasOne<Product>()
                .WithMany()
                .HasForeignKey(i => i.ProductId)
                .HasPrincipalKey(p => p.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
