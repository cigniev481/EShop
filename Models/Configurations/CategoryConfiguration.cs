using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.Models.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder
                .Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasData(new List<Category>
            {
                new Category { Id = 1, Title = "Azerbaijani" },
                new Category { Id = 2, Title = "Ukrainian" },
                new Category { Id = 3, Title = "Italian" },
                new Category { Id = 4, Title = "Japanese" }
            });
        }
    }
}
