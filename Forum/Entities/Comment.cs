namespace Forum.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public DateTime DateOfCreate { get; set; }
        public int TopicId { get; set; }
        public string Description { get; set; }
        public Topic Topic { get; set; }
    }
}