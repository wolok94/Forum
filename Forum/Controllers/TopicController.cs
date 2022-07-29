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
    public class TopicController : ControllerBase
    {
        private readonly ITopicService topicService;

        public TopicController(ITopicService topicService)
        {
            this.topicService = topicService;
        }
        [HttpPost]
        [Route("createTopic")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> Create([FromBody] TopicDto topic)
        {

            await topicService.Create(topic);
            return Ok();
        }
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await topicService.Delete(id);
            return NoContent();
        }
        [HttpPut]
        [Route("{id}/update")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateTopicDto dto)
        {
            await topicService.Update(id, dto);
            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationFilter paginationFilter)
        {
            return Ok(await topicService.GetAll(paginationFilter));
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            return Ok(await topicService.getTopicForId(id));
        }
    }
}
