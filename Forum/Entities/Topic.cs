namespace Forum.Entities
{
    public class Topic
    {
        public int Id { get; set; }
        public string NameOfTopic { get; set; }
        public string Description { get; set; }
        public DateTime DateOfCreate { get; set; } = DateTime.Now;
        public int UserId { get; set; }
        public  User User { get; set; }
        public List<Comment> Comments { get; set; } = new List<Comment>();

    }
}
