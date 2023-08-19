using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RWA_MVC_project.Models;

public partial class Tag
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Tag name is required")]
    public string Name { get; set; } = null!;

    public virtual ICollection<VideoTag> VideoTags { get; set; } = new List<VideoTag>();
}
