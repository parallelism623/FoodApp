using FoodShop.Domain.Entities;
using FoodShop.Persistence.Constrant;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodShop.Persistence.Configuration
{
    internal class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.ToTable(TableName.Cart);    
            builder.HasKey(c => c.Id);
            builder.HasMany(c => c.CartProducts)
                   .WithOne()
                   .HasForeignKey(ci => ci.CartId)
                   .IsRequired();
        }
    }
    internal class CartProductConfiguration : IEntityTypeConfiguration<CartProduct>
    {
        public void Configure(EntityTypeBuilder<CartProduct> builder)
        {
            builder.ToTable(TableName.CartProduct); 
            builder.HasKey(c => new {c.ProductId, c.CartId});
            builder.Property(c => c.IsSelected).HasDefaultValue(true);

        }
    }
}
