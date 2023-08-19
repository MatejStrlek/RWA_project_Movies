using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RWA_MVC_project.Models;

public partial class VideoTag
{
    public int Id { get; set; }

    [Required(ErrorMessage = "VideoId is required")]
    public int VideoId { get; set; }

    [Required(ErrorMessage = "TagId is required")]
    public int TagId { get; set; }

    public virtual Tag Tag { get; set; } = null!;

    public virtual Video Video { get; set; } = null!;
}
