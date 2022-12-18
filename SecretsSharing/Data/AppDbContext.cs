using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using SecretsSharing.Data.Models;
using System.Reflection.Emit;

namespace SecretsSharing.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<TextFile> TextFiles { get; set; }
        public DbSet<DocumentFile> DocumentFiles { get; set; }
    }
}
