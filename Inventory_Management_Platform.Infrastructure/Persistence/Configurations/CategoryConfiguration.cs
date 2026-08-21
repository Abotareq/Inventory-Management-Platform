using Inventory_Management_Platform.Domain.Category;
using Inventory_Management_Platform.Domain.Category.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Infrastructure.Persistence.Configurations
{
    public sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.CategoryId)
                .HasConversion(id => id.Value, value => CategoryId.Create(value))
                .HasColumnName("CategoryId")
                .ValueGeneratedNever();

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);
            builder.HasIndex(c => c.Name)
    .IsUnique();
        }
    }
}
