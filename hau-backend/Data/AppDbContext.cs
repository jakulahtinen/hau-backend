using hau_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace hau_backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<News> News { get; set; } = null!;
        public DbSet<Picture> Pictures { get; set; } = null!;
        public DbSet<Scores> Scores { get; set; } = null!;
        public DbSet<Folder> Folders { get; set; } = null!;
    }
}