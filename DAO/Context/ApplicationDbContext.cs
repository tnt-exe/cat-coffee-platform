using BusinessObject.Model;
using DAO.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DAO.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Area> Areas { get; set; } = default!;
        public DbSet<Bill> Bills { get; set; } = default!;
        public DbSet<Booking> Booking { get; set; } = default!;
        public DbSet<BookingProduct> BookingProducts { get; set; } = default!;
        public DbSet<Cat> Cats { get; set; } = default!;
        public DbSet<Category> Categories { get; set; } = default!;
        public DbSet<CoffeeShop> CoffeeShops { get; set; } = default!;
        public DbSet<Payment> Payments { get; set; } = default!;
        public DbSet<Product> Products { get; set; } = default!;
        public DbSet<TimeFrame> TimeFrames { get; set; } = default!;
        public DbSet<User> Users { get; set; } = default!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", true, true)
                    .Build();
                var connectionString = configuration.GetConnectionString("CatCoffeeShopDB");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder builder)
        {
            base.ConfigureConventions(builder);

            builder.Properties<DateOnly>()
                .HaveConversion<DateOnlyConverter, DateOnlyComparer>()
                .HaveColumnType("date");

            builder.Properties<TimeOnly>()
                .HaveConversion<TimeOnlyConverter, TimeOnlyComparer>();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookingProduct>(entity =>
            {
                entity.HasKey(e => new { e.BookingId, e.ProductId });
            });

            modelBuilder.Entity<CoffeeShop>(entity =>
            {
                entity.HasOne(mng => mng.Manager)
                    .WithOne(mng => mng.ManagedCoffeeShop)
                    .HasForeignKey<User>(entity => entity.ManagerShopId);

                entity.HasMany(entity => entity.Staffs)
                    .WithOne(entity => entity.CoffeeShop)
                    .HasForeignKey(entity => entity.CoffeeShopId);

                entity.HasMany(cat => cat.Cats)
                    .WithOne(cat => cat.CoffeeShop)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(area => area.Areas)
                .WithOne(area => area.CoffeeShop)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(tf => tf.TimeFrames)
                    .WithOne(tf => tf.CoffeeShop)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(product => product.Products)
                    .WithOne(product => product.CoffeeShop)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(booking => booking.Bookings)
                    .WithOne(booking => booking.CoffeeShop)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
