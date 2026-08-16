using Inventory_Management_Platform.Domain.Stock.Entites;
using Inventory_Management_Platform.Domain.Stock.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Infrastructure.Persistence.Configurations
{
    public sealed class StockAdjustmentConfiguration : IEntityTypeConfiguration<StockAdjustment>
    {
        public void Configure(EntityTypeBuilder<StockAdjustment> builder)
        {
            builder.ToTable("StockAdjustments");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.StockAdjustmentId)
                .HasConversion(id => id.Value, value => StockAdjustmentId.Create(value))
                .HasColumnName("StockAdjustmentId")
                .ValueGeneratedNever();

            builder.Property(a => a.StockId)
                .HasConversion(id => id.Value, value => StockId.Create(value))
                .HasColumnName("StockId");

            builder.Property(a => a.Delta)
                .IsRequired();

            builder.Property(a => a.ResultingQuantity)
                .IsRequired();

            builder.Property(a => a.Reason)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(a => a.PerformedByUserId)
                .IsRequired();

            builder.Property(a => a.Timestamp)
                .IsRequired();

            builder.HasIndex(a => a.StockId);
        }
    }
}
