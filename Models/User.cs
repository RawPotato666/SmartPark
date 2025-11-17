namespace SmartPark.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
public class User
{
    public int Id { get; set; }
    
    [Display(Name = "First Name")]
    [StringLength(50, MinimumLength = 3)]
    public string FirstName { get; set; } = null!;     // Ime

    [Display(Name = "Last Name")]
    [StringLength(50, MinimumLength = 2)]
    public string LastName { get; set; } = null!;      // Priimek

    [StringLength(50, MinimumLength = 6)]
    public string Username { get; set; }  = null!;     // Username

    // Relationships
    [DisplayFormat(NullDisplayText = "No reservations")]
    public ICollection<Reservation>? Reservations { get; set; }  // can have more
    public Administrator? Administrator { get; set; }            // is user an admin?
}