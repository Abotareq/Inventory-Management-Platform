using Inventory_Management_Platform.Domain.Product;
using Inventory_Management_Platform.Domain.Product.ValueObjects;
using Inventory_Management_Platform.Domain.Stock;
using Inventory_Management_Platform.Domain.Stock.ValueObjects;
using Inventory_Management_Platform.Domain.Warehouse;
using Inventory_Management_Platform.Domain.Warehouse.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Infrastructure.Persistence.Configurations
{

    public sealed class StockConfiguration : IEntityTypeConfiguration<Stock>
    {
        public void Configure(EntityTypeBuilder<Stock> builder)
        {
            builder.ToTable("Stocks");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.StockId)
                .HasConversion(id => id.Value, value => StockId.Create(value))
                .HasColumnName("StockId")
                .ValueGeneratedNever();

            builder.Property(s => s.ProductId)
                .HasConversion(id => id.Value, value => ProductId.Create(value))
                .HasColumnName("ProductId");

            builder.Property(s => s.WarehouseId)
                .HasConversion(id => id.Value, value => WarehouseId.Create(value))
                .HasColumnName("WarehouseId");

            builder.Property(s => s.Quantity)
                .IsRequired();

            builder.HasIndex(s => new { s.ProductId, s.WarehouseId })
                .IsUnique();

            builder.HasOne<Product>()
                .WithMany()
                .HasForeignKey(s => s.ProductId)
                .HasPrincipalKey(p => p.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Warehouse>()
                .WithMany()
                .HasForeignKey(s => s.WarehouseId)
                .HasPrincipalKey(w => w.WarehouseId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Property(s => s.RowVersion)
    .IsRowVersion();
        }
    }
}