namespace Forum.Models
{
    public class CommentDto
    {
        public DateTime DateOfCreate { get; set; } = DateTime.Now;
        public string Description { get; set; }
     
    }
}
