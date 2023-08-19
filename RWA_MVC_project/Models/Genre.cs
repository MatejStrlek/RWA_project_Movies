using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RWA_MVC_project.Models;

public partial class Genre
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Genre name is required")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Genre description is required")]
    public string? Description { get; set; }

    public virtual ICollection<Video> Videos { get; set; } = new List<Video>();
}
