using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TallerIdwm.src.models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TallerIdwm.src.data
{
    public class StoreContext(DbContextOptions<StoreContext> options) : IdentityDbContext<User>(options)
    {
        public required DbSet<Product> Products { get; set; }

        public required DbSet<ShippingAddress> ShippingAddres { get; set; }

        public required DbSet<Basket> Baskets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<User>()
                    .HasOne(u => u.ShippingAddress)
                    .WithOne(sa => sa.User)
                    .HasForeignKey<ShippingAddress>(sa => sa.UserId);
            List<IdentityRole> roles =
            [
                new IdentityRole { Id = "1" ,Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = "2" ,Name = "User", NormalizedName = "USER" }
            ];

            modelBuilder.Entity<IdentityRole>().HasData(roles);
        }
    }

}