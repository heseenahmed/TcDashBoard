using Microsoft.EntityFrameworkCore;
using TCDashBoard.Models;

namespace TCDashBoard
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Logos> Logos { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Works> Works { get; set; }
        public DbSet<Category> Categories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Works)
                .WithOne(w => w.Category)
                .HasForeignKey(w => w.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
