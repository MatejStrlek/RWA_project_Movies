using System.ComponentModel.DataAnnotations;

namespace RWA_MVC_project.Models;

public partial class Video
{
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; }

    [Required(ErrorMessage = "Video name is required")]
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    [Required(ErrorMessage = "Video genre is required")]
    public int GenreId { get; set; }

    [Required(ErrorMessage = "Video length is required")]
    public int TotalSeconds { get; set; }

    [RegularExpression(@"^(http|https)://[a-zA-Z0-9\-.]+\.[a-zA-Z]{2,3}(/\S*)?$", ErrorMessage = "Invalid URL")]
    public string? StreamingUrl { get; set; }

    public int? ImageId { get; set; }

    public virtual Genre Genre { get; set; } = null!;

    public virtual Image? Image { get; set; }

    public virtual ICollection<VideoTag> VideoTags { get; set; } = new List<VideoTag>();
}
