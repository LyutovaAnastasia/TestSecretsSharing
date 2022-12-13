using Microsoft.EntityFrameworkCore;
using SecretsSharing.Data.Models;

namespace SecretsSharing.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<TextFile> TextFiles { get; set; }
    }
}
