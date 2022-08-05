using System.ComponentModel.DataAnnotations;

namespace Forum.Models
{
    public class UserDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Nick { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
