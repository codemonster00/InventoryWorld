using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using InventoryWorld.Models;

namespace InventoryWorld.Models
{
  
        public class AppDbContext : IdentityDbContext<AppUser, AppRole, string>
        {

            public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
            {

            }
  
           public DbSet<Brands> Brands { get; set; }
        public DbSet<Stores> Stores { get; set; }

        public DbSet<Attributes> Attributes { get; set; }

        public DbSet<Products> Products { get; set; }
        public DbSet<ProductAttribute> ProductAttributes { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<Company> Company { get; set; }
  
public DbSet<InventoryWorld.Models.Order> Order { get; set; }
    }
    
}
