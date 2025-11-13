namespace SmartPark.Models;
public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;     // Ime
    public string LastName { get; set; } = null!;      // Priimek
    public string Username { get; set; }  = null!;     // Username

    // Relationships
    public ICollection<Reservation>? Reservations { get; set; }  // can have more
    public Administrator? Administrator { get; set; }            // is user an admin?
}