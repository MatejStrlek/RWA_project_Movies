using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RWA_MVC_project.Models;

public partial class Country
{
    public int Id { get; set; }

    [Display(Name = "Country code")]
    [Required(ErrorMessage = "Country code is required")]
    [StringLength(2, MinimumLength = 2, ErrorMessage = "Country code must be 2 characters long")]
    public string Code { get; set; } = null!;

    [Display(Name = "Country name")]
    [Required(ErrorMessage = "Country name is required")]
    public string Name { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
