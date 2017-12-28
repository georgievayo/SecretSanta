using System.ComponentModel.DataAnnotations;

namespace SecretSanta.API.Models
{
    public class CreateGroupViewModel
    {
        [Required]
        [MinLength(6)]
        public string Name { get; set; }
    }
}