using FoodShop.Domain.Entities;
using FoodShop.Persistence.Constrant;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Persistence.Configuration
{
    internal sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable(TableName.Product);
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).HasMaxLength(200).IsRequired(true);
            builder.Property(x => x.Description).HasMaxLength(250).IsRequired(true);
            builder.HasIndex(x => x.Code).IsUnique();
            builder.HasMany(x => x.CartProducts)
                   .WithOne()
                   .HasForeignKey(x => x.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.OrderProducts)
                   .WithOne()
                   .HasForeignKey(x => x.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.ProductCategories)
                   .WithOne()
                   .HasForeignKey(x => x.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
