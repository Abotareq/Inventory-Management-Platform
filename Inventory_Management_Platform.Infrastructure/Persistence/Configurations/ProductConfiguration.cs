using Inventory_Management_Platform.Domain.Category;
using Inventory_Management_Platform.Domain.Category.ValueObjects;
using Inventory_Management_Platform.Domain.Product;
using Inventory_Management_Platform.Domain.Product.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Infrastructure.Persistence.Configurations
{
    public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.ProductId)
                .HasConversion(id => id.Value, value => ProductId.Create(value))
                .HasColumnName("ProductId")
                .ValueGeneratedNever();

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.Sku)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(p => p.Sku)
                .IsUnique();

            builder.Property(p => p.Description)
                .HasMaxLength(1000);

            builder.Property(p => p.CategoryId)
                .HasConversion(
                    id => id == null ? (Guid?)null : id.Value,
                    value => value == null ? null : CategoryId.Create(value.Value))
                .HasColumnName("CategoryId");

            builder.HasOne<Category>()
                .WithMany()
                .HasForeignKey(p => p.CategoryId)
                .HasPrincipalKey(c => c.CategoryId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);
        }
    }
}
