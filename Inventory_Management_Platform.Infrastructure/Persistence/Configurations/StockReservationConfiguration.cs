using Inventory_Management_Platform.Domain.Order.ValueObjects;
using Inventory_Management_Platform.Domain.Stock.Entites;
using Inventory_Management_Platform.Domain.Stock.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Infrastructure.Persistence.Configurations
{

    public sealed class StockReservationConfiguration : IEntityTypeConfiguration<StockReservation>
    {
        public void Configure(EntityTypeBuilder<StockReservation> builder)
        {
            builder.ToTable("StockReservations");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.StockReservationId)
                .HasConversion(id => id.Value, value => StockReservationId.Create(value))
                .HasColumnName("StockReservationId")
                .ValueGeneratedNever();

            builder.Property(r => r.StockId)
                .HasConversion(id => id.Value, value => StockId.Create(value))
                .HasColumnName("StockId");

            builder.Property(r => r.OrderId)
                .HasConversion(id => id.Value, value => OrderId.Create(value))
                .HasColumnName("OrderId");

            builder.Property(r => r.Amount)
                .IsRequired();

            builder.Property(r => r.Action)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(r => r.PerformedByUserId)
                .IsRequired();

            builder.Property(r => r.Timestamp)
                .IsRequired();

            builder.HasIndex(r => r.StockId);
            builder.HasIndex(r => r.OrderId);
        }
    }

}
