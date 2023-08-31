using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RWA_MVC_project.Models;

public partial class Image
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Image name is required")]
    public string Content { get; set; } = null!;   

    public virtual ICollection<Video> Videos { get; set; } = new List<Video>();
}
