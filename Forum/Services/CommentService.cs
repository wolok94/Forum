using Forum.Authorization;
using Forum.Entities;
using Forum.Exceptions;
using Forum.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Forum.Services
{
    public interface ICommentService
    {
        Task Create(int id, CommentDto dto);
        Task Delete(int id);
    }

    public class CommentService : ICommentService
    {
        private readonly ForumDbContext dbContext;
        private readonly IUserContextService userContext;
        private readonly IAuthorizationService authorizationService;

        public CommentService(ForumDbContext dbContext, IUserContextService userContext, IAuthorizationService authorizationService)
        {
            this.dbContext = dbContext;
            this.userContext = userContext;
            this.authorizationService = authorizationService;
        }
        public async Task Create(int id, CommentDto dto)
        {
            Comment comment = new Comment()
            {
                UsernameThatCreatedComment = userContext.User.Identity.Name,
                DateOfCreate = dto.DateOfCreate,
                Description = dto.Description,
                TopicId = id,

            };
            await dbContext.Comments.AddAsync(comment);
            var topic = await dbContext.Topics.FirstOrDefaultAsync(t => t.Id == id);
            topic.Comments.Add(comment);
            await dbContext.SaveChangesAsync();
        }
        public async Task Delete(int id)
        {
            Comment comment = await dbContext.Comments.FirstOrDefaultAsync(c => c.Id == id);

            if (comment == null)
            {
                throw new NotFoundException("Comment not founded");
            }
            var authorizationResult = authorizationService.AuthorizeAsync(userContext.User, comment, new ResourceOperationRequirement(ResourceOperation.Delete)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException("You are unauthorized");
            }
            dbContext.Comments.Remove(comment);
            await dbContext.SaveChangesAsync();

        }
    }
}
