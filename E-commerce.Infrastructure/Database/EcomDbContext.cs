using E_commerce.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Infrastructure.Database
{
    public class EcomDbContext : IdentityDbContext<AppUser>
    {
        public EcomDbContext(DbContextOptions<EcomDbContext>option) : base (option)
        { }
       
        public DbSet<Product>Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Product → User
            builder.Entity<Product>()
                .HasOne(p => p.User)
                .WithMany(u => u.Products)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.NoAction);  // 

            // Product → Category
            builder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Category → User
            builder.Entity<Category>()
                .HasOne(c => c.User)
                .WithMany(u => u.ProductCategories)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);  //

            // Order → User
            builder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.ProductOrders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.NoAction);  // 

            // Order ↔ Products many-to-many
            builder.Entity<Order>()
                .HasMany(or => or.Products)
                .WithMany(p => p.ProductOrder);
        }


    }
}
