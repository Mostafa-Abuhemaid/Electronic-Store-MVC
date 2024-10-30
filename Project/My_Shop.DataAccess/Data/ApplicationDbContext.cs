using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using My_Shop.Entities.Models;



namespace My_Shop.DataAccess
{
    public class ApplicationDbContext :IdentityDbContext 
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options):base(options) {
        
        }
        public DbSet<Category>Categories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ApplicationUser> applicationUsers { get; set; }
        public DbSet<ShopingCard> ShopingCards { get; set; }

        public DbSet<OrderHeader> orderHeaders { get; set; }
        public DbSet<OrderDetail> orderDetails { get; set; }


    }
}
