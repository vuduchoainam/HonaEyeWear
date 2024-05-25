using HonaEyeWear.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace HonaEyeWear.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
        }
        public DbSet<Category> categories { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<Discount> discounts { get; set; }
        public DbSet<Order> orders { get; set; }
        public DbSet<OrderDetail> orderDetails { get; set; }
        public DbSet<Blog> blogs { get; set; }
        public DbSet<WishList> wishLists { get; set; }
        public DbSet<Property> properties { get; set; }
        public DbSet<PropertyValue> propertyValues { get; set; }
        public DbSet<PropertyProduct> propertyProducts { get; set; }
        public DbSet<Role> roles { get; set; }
        public DbSet<ProductImage> productImages { get; set; }




        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
