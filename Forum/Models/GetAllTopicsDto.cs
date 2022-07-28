using Forum.Entities;

namespace Forum.Models
{
    public class GetAllTopics
    {
        public string NameOfTopic { get; set; }
        public string Description { get; set; }
        public string User { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
