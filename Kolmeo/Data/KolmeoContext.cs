using Microsoft.EntityFrameworkCore;

namespace Kolmeo.Data
{
    public class KolmeoContext  : DbContext
    {
        public KolmeoContext() : base()
        {

        }

        public KolmeoContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>()
                .HasKey(p => p.Id);
        }
    }
}
