using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RWA_MVC_project.Models;

public partial class Notification
{
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; }

    [Required(ErrorMessage = "Receiver email is required")]
    public string ReceiverEmail { get; set; } = null!;

    [StringLength(50, ErrorMessage = "Subject cannot be longer than 50 characters.")]
    public string? Subject { get; set; }

    [Required(ErrorMessage = "Body is required")]
    public string Body { get; set; } = null!;

    public DateTime? SentAt { get; set; }
}
