namespace Forum.Models
{
    public class GetCommentsDto
    {
        public DateTime DateOfCreate { get; set; } = DateTime.Now;
        public string Description { get; set; }
        
    }
}
