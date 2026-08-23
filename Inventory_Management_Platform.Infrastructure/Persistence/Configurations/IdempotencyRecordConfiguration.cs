using Inventory_Management_Platform.Application.Common.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Infrastructure.Persistence.Configurations
{
    public sealed class IdempotencyRecordConfiguration : IEntityTypeConfiguration<IdempotencyRecord>
    {
        public void Configure(EntityTypeBuilder<IdempotencyRecord> builder)
        {
            builder.ToTable("IdempotencyRecords");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Key)
                .IsRequired()
                .HasMaxLength(200);

            builder.HasIndex(r => r.Key)
                .IsUnique();

            builder.Property(r => r.RequestType)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(r => r.ResponseData)
                .IsRequired();

            builder.Property(r => r.CreatedAt)
                .IsRequired();
        }
    }
}
