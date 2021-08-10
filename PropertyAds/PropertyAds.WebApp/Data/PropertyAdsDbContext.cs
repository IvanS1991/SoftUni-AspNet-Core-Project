namespace PropertyAds.WebApp.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using PropertyAds.WebApp.Data.Models;
    using System.Threading.Tasks;

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

        public DbSet<Watchlist> Watchlists { get; set; }

        public DbSet<WatchlistProperty> WatchlistProperties { get; set; }

        public DbSet<WatchlistPropertySegment> WatchlistPropertySegments { get; set; }

        public DbSet<Conversation> Conversations { get; set; }

        public DbSet<Message> Messages { get; set; }

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

            builder.Entity<WatchlistProperty>()
                .HasKey(x => new { x.WatchlistId, x.PropertyId });

            builder.Entity<WatchlistPropertySegment>()
                .HasKey(x => new { x.WatchlistId, x.PropertyTypeId, x.DistrictId });

            builder.Entity<User>()
                .HasMany(x => x.OwnConversations)
                .WithOne(x => x.Owner)
                .HasForeignKey(x => x.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<User>()
                .HasMany(x => x.RecipientConversations)
                .WithOne(x => x.Recipient)
                .HasForeignKey(x => x.RecipientId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }
    }
}
