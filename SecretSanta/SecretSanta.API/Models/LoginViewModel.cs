using System.ComponentModel.DataAnnotations;

namespace SecretSanta.API.Models
{
    public class LoginViewModel
    {
        [Required]
        [MinLength(5)]
        public string Username { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
}