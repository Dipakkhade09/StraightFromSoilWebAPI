using KhadeFarm_Web_API.Entity;
using Microsoft.EntityFrameworkCore;


namespace KhadeFarm_Web_API.DBContexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }

        // Add DbSet properties for your entities
        // public DbSet<Product> Products { get; set; }

        // Generic DbSet<T> access
        public DbSet<T> SetEntity<T>() where T : class => Set<T>();

        public DbSet<OurFeatures> Features { get; set; }

        public DbSet<OurProducts> Products { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Map entity class to table name
            modelBuilder.Entity<OurFeatures>().ToTable("KF_FEATURES");  // actual table name in DB

            // Map entity class to table name
            modelBuilder.Entity<OurProducts>().ToTable("KF_PRODUCTS");  // actual table name in DB
        }

    }
}
