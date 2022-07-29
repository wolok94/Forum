using Forum.Entities;
using Forum.Models;
using Forum.Pagination;
using Forum.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Controllers
{
    [ApiController]
    public class TopicController : ControllerBase
    {
        private readonly ITopicService topicService;

        public TopicController(ITopicService topicService)
        {
            this.topicService = topicService;
        }
        [HttpPost]
        [Route("api/createTopic")]
        [Authorize(Roles = "Admin, User")]
        public async Task <IActionResult> Create([FromBody] TopicDto topic)
        {
            
            await topicService.Create(topic);
            return Ok();
        }
        [HttpDelete]
        [Route("api/{id}")]
        public async Task <IActionResult> Delete([FromRoute] int id)
        {
            await topicService.Delete(id);
            return NoContent();
        }
        [HttpPut]
        [Route("api/{id}/update")]
        public async Task <IActionResult> Update([FromRoute] int id, [FromBody] UpdateTopicDto dto)
        {
            await topicService.Update(id, dto);
            return Ok();
        }
        [HttpGet]
        [Route("api/forum")]
        public async Task <IActionResult> GetAll([FromQuery] PaginationFilter paginationFilter)
        {

            var topics = await topicService.GetAll(paginationFilter);
            return Ok(topics);
        }
        [HttpGet]
        [Route("api/forum/{id}")]
        public async Task <IActionResult> GetForId([FromRoute] int id)
        {
            var topic = await topicService.getTopicForId(id);
            return Ok(topic);
        }
    }
}
