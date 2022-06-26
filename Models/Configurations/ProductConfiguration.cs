using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.Models.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder
                .Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder
                .Property(x => x.Price)
                .IsRequired();

            builder
                .Property(x => x.Description)
                .HasMaxLength(1000);
        }
    }
}
