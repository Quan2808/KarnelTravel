using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
using Model;

namespace WebApp.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            var tableName = entityType.GetTableName();
            if (tableName!.StartsWith("AspNet"))
            {
                entityType.SetTableName(tableName.Substring(6));
            }
        }

        builder.Entity<ApplicationUser>()
            .Property(u => u.PasswordHash)
            .HasColumnName("Password");

        builder.Entity<ApplicationUser>()
            .HasIndex(b => b.PhoneNumber)
            .IsUnique();

        builder.ApplyConfiguration(new ApplicationUserEntityConfiguration());
    }

    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<Resort> Resorts { get; set; }
    public DbSet<Restaurant> Restaurants { get; set; }
    public DbSet<TouristSpot> Tourists { get; set; }
    public DbSet<TravelInfo> Travels { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Rating> Ratings { get; set; }

    public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(u => u.FirstName).HasMaxLength(50);
            builder.Property(u => u.LastName).HasMaxLength(50);
            builder.Property(u => u.Address).HasMaxLength(255);
        }
    }
}
