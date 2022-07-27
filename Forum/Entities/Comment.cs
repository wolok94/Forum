namespace Forum.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string UsernameThatCreatedComment { get; set; }
        public DateTime DateOfCreate { get; set; }
        public int TopicId { get; set; }
        public virtual Topic Topic { get; set; }
    }
}