using FoodShop.Contract.Abstraction.Constrant;
using FoodShop.Domain.Entities.Identity;
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
    public class AppRoleConfiguration : IEntityTypeConfiguration<AppRole>
    {
        public void Configure(EntityTypeBuilder<AppRole> builder)
        {
            builder.ToTable(TableName.AppRole);
            builder.HasKey(t => t.Id);


            builder.Property(x => x.Description)
                   .HasMaxLength(250).IsRequired(true);
            builder.Property(x => x.RoleCode).HasMaxLength(50).IsRequired(true);

            builder.HasMany(r => r.Permissions)
                   .WithOne()
                   .HasForeignKey(p => p.RoleId)
                   .IsRequired();
            builder.HasMany(r => r.UserRoles)
                   .WithOne()
                   .HasForeignKey(p => p.RoleId)
                   .IsRequired();
            builder.HasMany(r => r.Claims)
                   .WithOne()
                   .HasForeignKey(c => c.RoleId)
                   .IsRequired();

        }
    }
}
