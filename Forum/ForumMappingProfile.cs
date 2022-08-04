using AutoMapper;
using Forum.Entities;
using Forum.Models;
using Forum.Pagination;

namespace Forum
{
    public class ForumMappingProfile : Profile
    {
        public ForumMappingProfile()
        {
            CreateMap<CreateUserDto, User>();
            CreateMap<User, GetAllUsersDto>();
            CreateMap<Topic, GetAllTopicsDto>()
                .ForMember(t => t.User, c => c.MapFrom(u => u.User.Nick));
            CreateMap<Comment, CommentDto>()
                .ForMember(c => c.UserName, c=> c.MapFrom(c=> c.User.Nick));
            CreateMap<Comment, GetCommentsDto>();
            CreateMap<PagedResult<Topic>, PagedResult<GetAllTopicsDto>>();
            CreateMap<Topic, TopicDto>()
                .ForMember(t => t.UserName, t => t.MapFrom(t => t.User.Nick));
            CreateMap<TopicDto, Topic>();
        }
    }
}
