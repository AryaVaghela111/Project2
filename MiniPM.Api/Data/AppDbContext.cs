using Microsoft.EntityFrameworkCore;
using MiniPM.Api.Models;

namespace MiniPM.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Project> Projects => Set<Project>();
        public DbSet<TaskItem> Tasks => Set<TaskItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // relations configured by conventions
        }
    }
}
