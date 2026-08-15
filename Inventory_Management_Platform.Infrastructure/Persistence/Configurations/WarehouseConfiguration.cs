using Inventory_Management_Platform.Domain.Warehouse;
using Inventory_Management_Platform.Domain.Warehouse.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Infrastructure.Persistence.Configurations
{
    public sealed class WarehouseConfiguration : IEntityTypeConfiguration<Warehouse>
    {
        public void Configure(EntityTypeBuilder<Warehouse> builder)
        {
            builder.ToTable("Warehouses");

            builder.HasKey(w => w.Id);

            builder.Property(w => w.WarehouseId)
                .HasConversion(id => id.Value, value => WarehouseId.Create(value))
                .HasColumnName("WarehouseId")
                .ValueGeneratedNever();

            builder.Property(w => w.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(w => w.Location)
                .IsRequired()
                .HasMaxLength(300);
        }
    }
}
