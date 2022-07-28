﻿using AutoMapper;
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
        void Create(TopicDto topicDto);
        public void Delete(int id);
        public void Update(int id, UpdateTopicDto dto);
        public List<List<GetComments>> GetAll();
        public Topic getTopicForId(int id);
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

        public void Create(TopicDto topicDto)
        {
            Topic topic = new Topic()
            {
                NameOfTopic = topicDto.NameOfTopic,
                Description = topicDto.Description,
                DateOfCreate = topicDto.DateOfCreate,
                UserId = context.GetId,
                Comments = topicDto.Comments,
            };
            dbContext.Topics.Add(topic);
            
            topicDto.UserId = context.GetId;
            dbContext.SaveChanges();
        }
        public void Delete(int id)
        {
            
            var topic = getTopicForId(id);
            var result = authorizationService.AuthorizeAsync(context.User, topic, new ResourceOperationRequirement(ResourceOperation.Delete)).Result;
            if (!result.Succeeded)
                throw new ForbidException("You are unauthorized");
            

            dbContext.Topics.Remove(topic);
            dbContext.SaveChanges();
        }
        public void Update(int id, UpdateTopicDto dto)
        {
            var topic = dbContext.Topics.FirstOrDefault(t => t.Id == id);
            var resultAuthorization = authorizationService.AuthorizeAsync(context.User, topic, new ResourceOperationRequirement(ResourceOperation.Update)).Result;

            if (!resultAuthorization.Succeeded)
                throw new ForbidException("You are unauthorized");
            
            topic.NameOfTopic = dto.Topic;
            topic.Description = dto.Description;

            dbContext.SaveChanges();
        }
        public List<List<GetComments>> GetAll()
        {
            var topics = dbContext
                .Topics
                .Include(c => c.Comments)
                .ToList();

            if (topics == null)
            {
                throw new NotFoundException("A list of topic is empty");
            }
            foreach (var topic in topics)
            {
                topic.User = dbContext.Users.FirstOrDefault(u => u.Id == topic.UserId);
            }

            var mappedTopics = mapper.Map<IEnumerable<GetAllTopics>>(topics);
            List<List<GetComments>> map = new List<List<GetComments>>();
            foreach (var topic in mappedTopics) {
                map = mapper.Map<List<List<GetComments>>>(topic.Comments);
                    }
            return map;
        }
        public Topic getTopicForId(int id)
        {
            var topic = dbContext
                .Topics
                .Include(c => c.Comments)
                .FirstOrDefault(x => x.Id == id);

            if (topic == null)
                throw new NotFoundException("Topic not found");
            

            return topic;
        }
    }
}
