using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Salil.Model;

namespace Salil.DataAccess
{
    public class ApplicationDBContext : IdentityDbContext 
    {
        // this constructor is very ciritcal as without this without this we can not create database and it won;t fetch the connection string for dbcontext
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
            
        }
        public DbSet<Category> categories { get; set; } 

        public DbSet<FoodType> foodTypes { get; set; }
        public DbSet<MenuItem> menuItems { get; set; }

        public DbSet<ApplicationUser> applicationUsers { get; set; }
        public DbSet<ShoppingCart> shoppingCarts { get; set; }
        public DbSet<OrderHeader> orderHeaders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }

    }
}
