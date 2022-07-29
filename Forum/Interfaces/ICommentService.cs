using Forum.Models;
using Forum.Pagination;

namespace Forum;

public interface ICommentService
{
    Task Create(int id, CommentDto dto);
    Task Delete(int id);
    Task<PagedResult<GetCommentsDto>> GetAll(PaginationFilter paginationFilter, int topicId);
}
