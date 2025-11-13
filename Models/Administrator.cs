namespace SmartPark.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
public class Administrator
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }
    public string? AdminName { get; set; }

    public ICollection<User>? Users { get; set; }  
}