using SmartPark.Models; // check later if it works after adding some models
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace SmartPark.Data;
public class SmartParkContext : DbContext
{
    public SmartParkContext(DbContextOptions<SmartParkContext> options) : base(options)
    {
            
    }
    public DbSet<ParkingLot> ParkingLots { get; set; }
    public DbSet<ParkingSpot> ParkingSpots { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Administrator> Administrators { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>().ToTable("User");
        modelBuilder.Entity<Administrator>().ToTable("Administrator");
        modelBuilder.Entity<ParkingLot>().ToTable("Parking Lot");
        modelBuilder.Entity<ParkingSpot>().ToTable("Parking Spot");
        modelBuilder.Entity<Reservation>().ToTable("Reservation");
    }
}