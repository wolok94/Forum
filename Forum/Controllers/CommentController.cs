using Forum.Entities;
using Forum.Models;
using Forum.Pagination;
using Forum.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Controllers
{
    [ApiController]
    [Route("api/forum/")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService commentService;

        public CommentController(ICommentService commentService)
        {
            this.commentService = commentService;
        }
        [HttpPost]
        [Route("{id}/comments")]
        [Authorize(Roles = "Admin, User")]
        public async Task <IActionResult> Create([FromRoute] int id, [FromBody] CommentDto dto)
        {
            await commentService.Create(id, dto);
            return Ok();
        }
        [HttpDelete]
        [Route("{id}/comments")]
        public async Task <IActionResult> Delete ([FromRoute] int id)
        {
            await commentService.Delete(id);
            return Ok();
        }
        [HttpGet]
        [Route("{topicId}/comments")]
        public async Task <IActionResult> GetAll([FromRoute] int topicId, [FromQuery] PaginationFilter paginationFilter)
        {
            return Ok(await commentService.GetAll(paginationFilter, topicId));
        }
        [HttpPut]
        [Route("comments/{commentId}")]
        public async Task <IActionResult> Update ([FromRoute] int commentId, [FromBody] string description)
        {
            await commentService.Update(commentId, description);
            return Ok();
        }

    }
}
