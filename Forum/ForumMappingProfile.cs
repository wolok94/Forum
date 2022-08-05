using AutoMapper;
using Forum.Entities;
using Forum.Pagination;
using Forum.Models;
namespace Forum
{
    public class ForumMappingProfile : Profile
    {
        public ForumMappingProfile()
        {
            CreateMap<CreateUserDto, User>();
            CreateMap<User, UserDto>();
            CreateMap<Comment, CommentDto>()
                .ForMember(c => c.UserName, c=> c.MapFrom(c=> c.User.Nick));
            CreateMap<PagedResult<Topic>, PagedResult<TopicDto>>();
            CreateMap<Topic, TopicDto>()
                .ForMember(t => t.UserName, t => t.MapFrom(t => t.User.Nick));
            CreateMap<TopicDto, Topic>();
        }
    }
}
