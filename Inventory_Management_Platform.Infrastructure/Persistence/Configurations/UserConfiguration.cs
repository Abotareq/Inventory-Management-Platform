using Inventory_Management_Platform.Domain.User;
using Inventory_Management_Platform.Domain.User.Entites;
using Inventory_Management_Platform.Domain.User.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Infrastructure.Persistence.Configurations
{
    public sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.UserId)
                .HasConversion(id => id.Value, value => UserId.Create(value))
                .HasColumnName("UserId")
                .ValueGeneratedNever();

            builder.Property(u => u.FullName).IsRequired().HasMaxLength(200);
            builder.Property(u => u.Email).IsRequired().HasMaxLength(256);
        }
    }

    public sealed class AdministratorConfiguration : IEntityTypeConfiguration<Administrator>
    {
        public void Configure(EntityTypeBuilder<Administrator> builder)
        {
            builder.ToTable("Administrators"); // TPT — separate table, joined to Users via shared key
        }
    }

    public sealed class WarehouseOperatorConfiguration : IEntityTypeConfiguration<WarehouseOperator>
    {
        public void Configure(EntityTypeBuilder<WarehouseOperator> builder)
        {
            builder.ToTable("WarehouseOperators");
        }
    }

    public sealed class ManagerConfiguration : IEntityTypeConfiguration<Manager>
    {
        public void Configure(EntityTypeBuilder<Manager> builder)
        {
            builder.ToTable("Managers");
        }
    }
}
