using NuGet.Protocol.Core.Types;
using System.ComponentModel.DataAnnotations;

namespace RWA_MVC_project.Models
{
    public partial class LoginUser
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string? Phone { get; set; } = null!;

        [Display(Name = "Country of residence")]
        public int CountryOfResidenceId { get; set; }
    }
}
