using AutoMapper;
using Forum.Authorization;
using Forum.Entities;
using Forum.Exceptions;
using Forum.Models;
using Forum.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Forum.Services
{


    public class CommentService : ICommentService
    {
        private readonly ForumDbContext dbContext;
        private readonly IUserContextService userContext;
        private readonly IAuthorizationService authorizationService;
        private readonly IMapper mapper;

        public CommentService(ForumDbContext dbContext, IUserContextService userContext, IAuthorizationService authorizationService, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.userContext = userContext;
            this.authorizationService = authorizationService;
            this.mapper = mapper;
        }
        public async Task Create(int id, CommentDto dto)
        {
            Comment comment = new Comment()
            {
                UserId = (int)userContext.GetId,
                DateOfCreate = dto.DateOfCreate,
                Description = dto.Description,
                TopicId = id,

            };
            await dbContext.Comments.AddAsync(comment);

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
        public async Task<PagedResult<CommentDto>> GetAll(PaginationFilter paginationFilter, int topicId)
        {
            
            var basicQuery = dbContext.Comments
                .AsNoTracking()
                .Include(u => u.User)
                .Where(r => r.TopicId == topicId &&
                (paginationFilter.SearchPhrase == null || r.Description == paginationFilter.SearchPhrase));

            if (!string.IsNullOrEmpty(paginationFilter.SortBy))
            {
                var dictionary = new Dictionary<string, Expression<Func<Comment, object>>>
                {
                    {nameof (Comment.DateOfCreate), c => c.DateOfCreate },
                    {nameof (Comment.User), c=> c.User }
                };

                var selectedColumns = dictionary[paginationFilter.SortBy];
                basicQuery = paginationFilter.SortDirection == SortDirection.ASC ?
                   basicQuery.OrderBy(selectedColumns) : basicQuery.OrderByDescending(selectedColumns);
            }

            var comments = basicQuery
                .Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
                .Take(paginationFilter.PageSize)
                .ToList();

            var totalItemsCount = basicQuery.Count();

            if (!comments.Any())
            {
                throw new NotFoundException("Comments not founded");
            }

            var mappedComments = mapper.Map<List<CommentDto>>(comments);

            var pagedResult = new PagedResult<CommentDto>(mappedComments, paginationFilter.PageSize, paginationFilter.PageNumber, totalItemsCount);

            return pagedResult;
        }

        public async Task Update(int commentId, string description)
        {
            var comment = await dbContext.Comments.FirstOrDefaultAsync(c => c.Id == commentId);
            comment.Description = description;
            await dbContext.SaveChangesAsync();

        }
    }
}
