using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

namespace ShopApp
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }
        public DbSet<Item> Items { get; set; }
        public DbSet<CartItem> Cart { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Account> Accounts => Set<Account>();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=store.db");
        }

    }
}
