namespace SmartPark.Models;
public class ParkingSpot
{
    public int Id { get; set; }
    public bool IsDisabled { get; set; }        // Invalidno
    public int ParkingLotId { get; set; }
    public bool IsOccupied { get; set; }       // Zauzeto

    // Relationships
    public ParkingLot ParkingLot { get; set; } = null!; // Which lot it belongs to
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();  // All reservations for this spot
}