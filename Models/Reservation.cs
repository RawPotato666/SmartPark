namespace SmartPark.Models;
public class Reservation
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ParkingSpotId { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }

    // Relationships
    public User User { get; set; } = null!;         // goes to an user
    public ParkingSpot ParkingSpot { get; set; }  = null!; // Which spot is reserved
}