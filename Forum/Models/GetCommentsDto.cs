namespace Forum.Models
{
    public class GetComments
    {
        public DateTime DateOfCreate { get; set; } = DateTime.Now;
        public string Description { get; set; }
        public string UserNameThatCreateTopic { get; set; }
    }
}
