using AutoMapper;
using Forum.Entities;
using Forum.Models;

namespace Forum
{
    public class ForumMappingProfile : Profile
    {
        public ForumMappingProfile()
        {
            CreateMap<CreateUserDto, User>();
            CreateMap<User, GetAllUsersDto>();
            CreateMap<Topic, GetAllTopics>()
                .ForMember(t => t.User, c => c.MapFrom(u => u.User.Nick));
            CreateMap<Comment, CommentDto>();
            CreateMap<Comment, GetComments>();
        }
    }
}
