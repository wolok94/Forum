using AutoMapper;
using Forum.Authorization;
using Forum.Entities;
using Forum.Exceptions;
using Forum.Models;
using Forum.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Forum.Services
{
    public interface ITopicService
    {
        Task Create(TopicDto topicDto);
        public Task Delete(int id);
        public Task Update(int id, UpdateTopicDto dto);
        public Task<PagedResult<GetAllTopicsDto>> GetAll(PaginationFilter paginationFilter);
        public Task <Topic> getTopicForId(int id);
    }

    public class TopicService : ITopicService
    {
        private readonly ForumDbContext dbContext;
        private readonly IUserContextService context;
        private readonly IAuthorizationService authorizationService;
        private readonly IMapper mapper;

        public TopicService(ForumDbContext dbContext, IUserContextService context, IAuthorizationService authorizationService, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.context = context;
            this.authorizationService = authorizationService;
            this.mapper = mapper;
        }

        public async Task Create(TopicDto topicDto)
        {
            Topic topic = new Topic()
            {
                NameOfTopic = topicDto.NameOfTopic,
                Description = topicDto.Description,
                DateOfCreate = topicDto.DateOfCreate,
                UserId = context.GetId,
                Comments = topicDto.Comments,
            };
            await dbContext.Topics.AddAsync(topic);
            
            topicDto.UserId = context.GetId;
            await dbContext.SaveChangesAsync();
        }
        public async Task Delete(int id)
        {
            

            var topic = await  getTopicForId(id);
            var result = authorizationService.AuthorizeAsync(context.User, topic, new ResourceOperationRequirement(ResourceOperation.Delete)).Result;
            if (!result.Succeeded)
                throw new ForbidException("You are unauthorized");
            

            dbContext.Topics.Remove(topic);
            await dbContext.SaveChangesAsync();
        }
        public async Task Update(int id, UpdateTopicDto dto)
        {
            var topic = await dbContext.Topics.FirstOrDefaultAsync(t => t.Id == id);
            var resultAuthorization = authorizationService.AuthorizeAsync(context.User, topic, new ResourceOperationRequirement(ResourceOperation.Update)).Result;

            if (!resultAuthorization.Succeeded)
                throw new ForbidException("You are unauthorized");
            
            topic.NameOfTopic = dto.Topic;
            topic.Description = dto.Description;

            await dbContext.SaveChangesAsync();
        }
        public async Task <PagedResult<GetAllTopicsDto>> GetAll(PaginationFilter paginationFilter)
        {
            

            var basicQuery = await dbContext
                .Topics
                .Include(c => c.Comments)
                .Where(r => paginationFilter.SearchPhrase == null
                || r.NameOfTopic.ToLower().Contains(paginationFilter.SearchPhrase.ToLower())
                || r.Description.ToLower().Contains(paginationFilter.SearchPhrase.ToLower()))
                .ToListAsync();


            var topics = basicQuery
                .Skip((paginationFilter.PageNumber -1) * paginationFilter.PageSize)
                .Take(paginationFilter.PageSize)
                .ToList();

            var totalItemsCount = basicQuery.Count();

            

            if (topics == null)
            {
                throw new NotFoundException("A list of topic is empty");
            }
            foreach (var topic in topics)
            {
                topic.User = await  dbContext.Users.FirstOrDefaultAsync(u => u.Id == topic.UserId);
            }

            

            var mappedTopics = mapper.Map<List<GetAllTopicsDto>>(topics);

            var pagedResult = new PagedResult<GetAllTopicsDto>(mappedTopics, paginationFilter.PageSize, paginationFilter.PageNumber, totalItemsCount);



            return pagedResult;
        }
        public async Task <Topic> getTopicForId(int id)
        {
            var topic = await dbContext
                .Topics
                .Include(c => c.Comments)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (topic == null)
                throw new NotFoundException("Topic not found");
            

            return topic;
        }
    }
}
