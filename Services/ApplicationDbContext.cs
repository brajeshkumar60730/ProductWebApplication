using Microsoft.EntityFrameworkCore;
using ProductWebApplication.Models;

namespace ProductWebApplication.Services
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Product>Products { get; set; }
    }
}
