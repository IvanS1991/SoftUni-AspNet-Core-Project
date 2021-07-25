namespace PropertyAds.WebApp.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using PropertyAds.WebApp.Data.Models;

    public class PropertyAdsDbContext : IdentityDbContext<User>
    {
        public PropertyAdsDbContext(DbContextOptions<PropertyAdsDbContext> options)
            : base(options) { }

        public override DbSet<User> Users { get; set; }

        public DbSet<Property> Properties { get; set; }

        public DbSet<PropertyType> PropertyTypes { get; set; }

        public DbSet<District> Districts { get; set; }

        public DbSet<PropertyAggregate> PropertyAggregates { get; set; }

        public DbSet<PropertyImage> PropertyImages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<PropertyAggregate>()
                .HasKey(x => new { x.DistrictId, x.PropertyTypeId });

            builder.Entity<Property>()
                .Property(x => x.Area)
                .HasColumnType("decimal(6, 2)");

            builder.Entity<Property>()
                .Property(x => x.UsableArea)
                .HasColumnType("decimal(6, 2)");

            builder.Entity<Property>()
                .HasOne(x => x.Owner)
                .WithMany(x => x.Properties)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }
    }
}
