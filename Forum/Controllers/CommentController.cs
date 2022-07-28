using Forum.Entities;
using Forum.Models;
using Forum.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Controllers
{
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService commentService;

        public CommentController(ICommentService commentService)
        {
            this.commentService = commentService;
        }
        [HttpPost]
        [Route("api/forum/{id}/comments")]
        [Authorize(Roles = "Admin, User")]
        public async Task <IActionResult> Create([FromRoute] int id, [FromBody] CommentDto dto)
        {
            await commentService.Create(id, dto);
            return Ok();
        }
        [HttpDelete]
        [Route("api/foum/{id}/comments")]
        public async Task <IActionResult> Delete ([FromRoute] int id)
        {
            await commentService.Delete(id);
            return Ok();
        }
    }
}
