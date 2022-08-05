using Forum.Entities;

namespace Forum.Models;

public class CommentDto
{
    public DateTime DateOfCreate { get; set; } = DateTime.Now;
    public string Description { get; set; }
    public string UserName { get; set; }

}
