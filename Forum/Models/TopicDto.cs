using Forum.Entities;

namespace Forum.Models
{
    public class TopicDto
    {
        public string NameOfTopic { get; set; }
        public string Description { get; set; }
        public DateTime DateOfCreate { get; set; } = DateTime.Now;
        public int? UserId { get; set; }
        public User? User { get; set; }

    }
}
