using AutoMapper;
using Forum.Authorization;
using Forum.Entities;
using Forum.Exceptions;
using Forum.Models;
using Forum.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Forum.Services;


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
            UserId = (int)context.GetId,
            
        };
        await dbContext.Topics.AddAsync(topic);
        await dbContext.SaveChangesAsync();
    }
    public async Task Delete(int id)
    {
        var topic = await dbContext.Topics.FirstOrDefaultAsync(t => t.UserId == id);
        if (topic == null)
        {
            throw new NotFoundException("Topic not founded");
        }
        var result = authorizationService.AuthorizeAsync(context.User, topic, new ResourceOperationRequirement(ResourceOperation.Delete)).Result;
        if (!result.Succeeded)
        {
            throw new ForbidException("You are unauthorized");
        }
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
    public async Task<PagedResult<TopicDto>> GetAll(PaginationFilter paginationFilter)
    {
        var basicQuery = dbContext
            .Topics
            .AsNoTracking()
            .Include(c => c.Comments)
            .ThenInclude(c => c.User)
            .Include(u => u.User)
            .Where(r => paginationFilter.SearchPhrase == null
            || r.NameOfTopic.ToLower().Contains(paginationFilter.SearchPhrase.ToLower())
            || r.Description.ToLower().Contains(paginationFilter.SearchPhrase.ToLower()));
            

        var topics = await basicQuery
            .Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
            .Take(paginationFilter.PageSize)
            .ToListAsync();

        var totalItemsCount = basicQuery.Count();
        if (!topics.Any())
        {
            throw new NotFoundException("A list of topic is empty");
        }
        var mappedTopics = mapper.Map<List<TopicDto>>(topics);
        var pagedResult = new PagedResult<TopicDto>(mappedTopics, paginationFilter.PageSize, paginationFilter.PageNumber, totalItemsCount);
        return pagedResult;
    }

    public async Task<TopicDto> getTopicForId(int id)
    {
        var topic = await dbContext
            .Topics
            .AsNoTracking()
            .Include(c => c.Comments)
            .ThenInclude(c => c.User)
            .Include(u => u.User)
            .FirstOrDefaultAsync(x => x.Id == id);

        var topicDto = mapper.Map<TopicDto>(topic);

        if (topic == null)
            throw new NotFoundException("Topic not found");
         return topicDto;
    }
}
