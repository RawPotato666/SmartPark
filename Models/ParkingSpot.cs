namespace SmartPark.Models;
public class ParkingSpot
{
    public int Id { get; set; }
    public bool IsDisabled { get; set; }        // Invalidno
    public int ParkingLotId { get; set; }

    // Relationships
    public ParkingLot ParkingLot { get; set; } = null!; // Which lot it belongs to
    public ICollection<Reservation>? Reservations { get; set; }  // All reservations for this spot
}