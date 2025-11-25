using SmartPark.Models; // check later if it works after adding some models
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace SmartPark.Data;
public class SmartParkContext : IdentityDbContext<User>
{
    public SmartParkContext(DbContextOptions<SmartParkContext> options) : base(options)
    {
            
    }
    public DbSet<ParkingLot> ParkingLots { get; set; }
    public DbSet<ParkingSpot> ParkingSpots { get; set; }
    public DbSet<Reservation> Reservations { get; set; }

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<ParkingLot>().ToTable("ParkingLot");
        modelBuilder.Entity<ParkingSpot>().ToTable("ParkingSpot");
        modelBuilder.Entity<Reservation>().ToTable("Reservation");

        // Relationships
        modelBuilder.Entity<Reservation>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reservations)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Reservation>()
                .HasOne(r => r.ParkingSpot)
                .WithMany(p => p.Reservations)
                .HasForeignKey(r => r.ParkingSpotId)
                .OnDelete(DeleteBehavior.Cascade);
    }
}