using Forum.Models;
using Forum.Pagination;

namespace Forum;

public interface ICommentService
{
    Task Create(int id, CommentDto dto);
    Task Delete(int id);
    Task<PagedResult<CommentDto>> GetAll(PaginationFilter paginationFilter, int topicId);
    Task Update(int commentId, string description);
}
