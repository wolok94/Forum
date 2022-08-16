using System.ComponentModel.DataAnnotations;

namespace Forum.Models
{
    public class LoginDto
    {
        [Required]
        public string Nick { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
