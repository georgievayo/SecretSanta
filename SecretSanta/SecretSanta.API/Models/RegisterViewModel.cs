using System.ComponentModel.DataAnnotations;

namespace SecretSanta.API.Models
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public string Username { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public int Age { get; set; }

        public string Interests { get; set; }

        public string PhoneNumber { get; set; }

        public string DisplayName { get; set; }

        public string Address { get; set; }
    }

}
