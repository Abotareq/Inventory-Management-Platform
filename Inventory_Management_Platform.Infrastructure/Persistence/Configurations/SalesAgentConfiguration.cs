using Inventory_Management_Platform.Domain.User.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_Management_Platform.Infrastructure.Persistence.Configurations
{
    public sealed class SalesAgentConfiguration : IEntityTypeConfiguration<SalesAgent>
    {
        public void Configure(EntityTypeBuilder<SalesAgent> builder)
        {
            builder.ToTable("SalesAgents");
        }
    }
}
