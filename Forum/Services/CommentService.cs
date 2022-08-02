using AutoMapper;
using Forum.Authorization;
using Forum.Entities;
using Forum.Exceptions;
using Forum.Models;
using Forum.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

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
        public async Task<PagedResult<GetCommentsDto>> GetAll(PaginationFilter paginationFilter, int topicId)
        {

            var basicQuery = await dbContext.Comments
                .AsNoTracking()
                .Where(r => r.TopicId == topicId && 
                paginationFilter.SearchPhrase == null || r.Description == paginationFilter.SearchPhrase)
                .ToListAsync();

            var comments = basicQuery
                .Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
                .Take(paginationFilter.PageSize)
                .ToList();

            var totalItemsCount = basicQuery.Count;

            if (comments == null)
            {
                throw new NotFoundException("Comments not founded");
            }

            var mappedComments = mapper.Map<List<GetCommentsDto>>(comments);

            var pagedResult = new PagedResult<GetCommentsDto>(mappedComments, paginationFilter.PageSize, paginationFilter.PageNumber, totalItemsCount);

            return pagedResult;

            
        }
    }
}
