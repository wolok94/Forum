using Forum.Entities;
using Forum.Models;
using Forum.Pagination;

namespace Forum;

public interface ITopicService
{
    Task Create(TopicDto topicDto);
    Task Delete(int id);
    Task<PagedResult<GetAllTopicsDto>> GetAll(PaginationFilter paginationFilter);
    Task<TopicDto> getTopicForId(int id);
    Task Update(int id, UpdateTopicDto dto);
}
