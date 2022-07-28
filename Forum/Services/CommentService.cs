using Forum.Entities;
using Forum.Models;

namespace Forum.Services
{
    public interface ICommentService
    {
        void Create(int id, CommentDto dto);
    }

    public class CommentService : ICommentService
    {
        private readonly ForumDbContext dbContext;
        private readonly IUserContextService userContext;

        public CommentService(ForumDbContext dbContext, IUserContextService userContext)
        {
            this.dbContext = dbContext;
            this.userContext = userContext;
        }
        public void Create(int id, CommentDto dto)
        {
            Comment comment = new Comment()
            {
                UsernameThatCreatedComment = userContext.User.Identity.Name,
                DateOfCreate = dto.DateOfCreate,
                Description = dto.Description,
                TopicId = id,

            };
            dbContext.Comments.Add(comment);
            var topic = dbContext.Topics.FirstOrDefault(t => t.Id == id);
            topic.Comments.Add(comment);
            dbContext.SaveChanges();
        }
    }
}
