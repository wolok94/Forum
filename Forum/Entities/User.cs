using System.Drawing;

namespace Forum.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Nick { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PasswordHash { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? ImagePath { get; set; }
        public List<Topic> Topics { get; set; } = new List<Topic>();
        public List<Comment> Comments { get; set; } = new List<Comment>();
        public Role Role { get; set; }
        public int RoleId { get; set; }
        

    }
}
