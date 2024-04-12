using FoodShop.Domain.Entities.Identity;
using FoodShop.Persistence.Constrant;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodShop.Persistence.Configuration
{
    internal sealed class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.ToTable(TableName.AppUser);

            builder.HasKey(x => x.Id);


            builder.HasMany(e => e.Claims)
                .WithOne()
                .HasForeignKey(uc => uc.UserId)
                .IsRequired();


            builder.HasMany(e => e.Logins)
                .WithOne()
                .HasForeignKey(ul => ul.UserId)
                .IsRequired();


            builder.HasMany(e => e.Tokens)
                .WithOne()
                .HasForeignKey(ut => ut.UserId)
                .IsRequired();


            builder.HasMany(e => e.UserRoles)
                .WithOne()
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
            builder.HasMany(e => e.Carts)
                   .WithOne()
                   .HasForeignKey(c => c.UserId);
            builder.HasMany(e => e.Orders)
                   .WithOne()
                   .HasForeignKey(o => o.UserId);
        }
    }
}
