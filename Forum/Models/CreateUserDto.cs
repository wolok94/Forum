using Forum.Entities;
using System.ComponentModel.DataAnnotations;

namespace Forum.Models
{

    public class CreateUserDto
    {
        
        [Required]
        public string Email { get; set; }
        [Required]
        public string Nick { get; set; }
        [Required]
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }

        public int RoleId { get; set; } = 1;

    }
}
