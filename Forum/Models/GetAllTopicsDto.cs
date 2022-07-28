using Forum.Entities;

namespace Forum.Models
{
    public class GetAllTopicsDto
    {
        public string NameOfTopic { get; set; }
        public string Description { get; set; }
        public string User { get; set; }
        public List<GetCommentsDto> Comments { get; set; }
    }
}
