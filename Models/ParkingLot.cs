namespace SmartPark.Models;

public class ParkingLot
{
    public int Id { get; set; }
    public string Location { get; set; } = null!;           
    public int Capacity { get; set; }          
    public int DisabledSpots { get; set; }      

    // Relationships
    public ICollection<ParkingSpot> ParkingSpots { get; set; } = null!;  // A parking lot has many parking spots
}