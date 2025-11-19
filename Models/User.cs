namespace SmartPark.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
public class User : IdentityUser
{    
    [Display(Name = "First Name")]
    [StringLength(50, MinimumLength = 3)]
    public string FirstName { get; set; } = null!;     // Ime

    [Display(Name = "Last Name")]
    [StringLength(50, MinimumLength = 2)]
    public string LastName { get; set; } = null!;      // Priimek

    // Relationships
    [DisplayFormat(NullDisplayText = "No reservations")]
    public ICollection<Reservation>? Reservations { get; set; }  // can have more

}