namespace SmartPark.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
public class Reservation
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ParkingSpotId { get; set; }

    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
    public DateTime Start { get; set; }

    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
    public DateTime End { get; set; }

    // Relationships
    public User User { get; set; } = null!;         // goes to an user
    public ParkingSpot ParkingSpot { get; set; }  = null!; // Which spot is reserved
}