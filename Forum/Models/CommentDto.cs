using Forum.Entities;
using System.ComponentModel.DataAnnotations;

namespace Forum.Models;

public class CommentDto
{
    public DateTime DateOfCreate { get; set; } = DateTime.Now;
    [Required]
    public string Description { get; set; }
    public string UserName { get; set; }

}
