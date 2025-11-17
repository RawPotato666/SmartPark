namespace SmartPark.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
public class Administrator
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }

    [Display(Name = "Administrator")]
    [StringLength(50)]
    public string? AdminName { get; set; }

    public ICollection<User>? Users { get; set; }  
}