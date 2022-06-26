using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.Models.Configurations
{
    public class OrderProductConfiguration : IEntityTypeConfiguration<OrderProduct>
    {
        public void Configure(EntityTypeBuilder<OrderProduct> builder)
        {
            builder
                .HasOne(pt => pt.Order)
                .WithMany(p => p.OrderProducts)
                .HasForeignKey(pt => pt.OrderId);

            builder
                .HasOne(pt => pt.Product)
                .WithMany(t => t.OrderProducts)
                .HasForeignKey(pt => pt.ProductId);
        }
    }
}
