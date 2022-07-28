using Forum.Entities;
using Forum.Models;
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
        public ActionResult Delete([FromRoute] int id)
        {
            topicService.Delete(id);
            return NoContent();
        }
        [HttpPut]
        [Route("api/{id}/update")]
        public ActionResult Update([FromRoute] int id, [FromBody] UpdateTopicDto dto)
        {
            topicService.Update(id, dto);
            return Ok();
        }
        [HttpGet]
        [Route("api/forum")]
        public ActionResult GetAll()
        {
            var topics = topicService.GetAll();
            return Ok(topics);
        }
        [HttpGet]
        [Route("api/forum/{id}")]
        public ActionResult GetForId([FromRoute] int id)
        {
            var topic = topicService.getTopicForId(id);
            return Ok(topic);
        }
    }
}
