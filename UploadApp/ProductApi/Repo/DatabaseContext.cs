using Microsoft.EntityFrameworkCore;
using ProductApi.Models;

namespace ProductApi.Repo
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public DbSet<Product> Product { get; set; }
    }
}
