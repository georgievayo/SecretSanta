using System.ComponentModel.DataAnnotations;

namespace SecretSanta.API.Models
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The username must be at least {2} characters long.", MinimumLength = 5)]
        public string Username { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The password must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public int Age { get; set; }

        public string Interests { get; set; }

        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The display name must be at least {2} characters long.", MinimumLength = 10)]
        public string DisplayName { get; set; }

        public string Address { get; set; }
    }

}
