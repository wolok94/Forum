using AutoMapper;
using Forum.Authorization;
using Forum.Entities;
using Forum.Exceptions;
using Forum.Models;
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
        public Task <IEnumerable<GetAllTopicsDto>> GetAll();
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
        public async Task <IEnumerable<GetAllTopicsDto>> GetAll()
        {
            var topics = await  dbContext
                .Topics
                .Include(c => c.Comments)
                .ToListAsync();

            if (topics == null)
            {
                throw new NotFoundException("A list of topic is empty");
            }
            foreach (var topic in topics)
            {
                topic.User = await  dbContext.Users.FirstOrDefaultAsync(u => u.Id == topic.UserId);
            }

            

            var mappedTopics = mapper.Map<IEnumerable<GetAllTopicsDto>>(topics);

        

            return mappedTopics;
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
