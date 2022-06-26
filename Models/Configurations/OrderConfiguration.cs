using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.Models.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder
                .Property(x => x.FullName)
                .HasMaxLength(100)
                .IsRequired();

            builder
                .Property(x => x.Phone)
                .HasMaxLength(100)
                .IsRequired();

            builder
                .Property(x => x.Email)
                .HasMaxLength(100)
                .IsRequired();

            builder
                .Property(x => x.Address)
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}
