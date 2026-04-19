using Microsoft.EntityFrameworkCore;
using SmartLibraryPro.Models;

namespace SmartLibraryPro.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books => Set<Book>();
        public DbSet<Member> Members => Set<Member>();
        public DbSet<Transaction> Transactions => Set<Transaction>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Default AvailableQuantity = Quantity
            modelBuilder.Entity<Book>()
                .Property(b => b.AvailableQuantity)
                .HasDefaultValue(0);
        }
    }
}