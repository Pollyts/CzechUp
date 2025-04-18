using CzechUp.EF.Models;
using CzechUp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CzechUp.WebApi.Controllers
{
    public class TopicController : BaseController
    {
        private readonly ITopicService topicService;
        public TopicController(ITopicService topicService)
        {
            this.topicService = topicService;
        }
        [HttpGet]
        public async Task<IActionResult> GetTopics(CancellationToken cancellationToken)
        {
            var topics = await this.topicService.GetTopics(UserGuid(), cancellationToken);
            return Ok(topics);
        }

        [HttpGet("topic")]
        public async Task<IActionResult> GetTopics(Guid guid, CancellationToken cancellationToken)
        {
            var topic = await this.topicService.GetTopic(guid, cancellationToken);
            return Ok(topic);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTopic([FromBody] UserTopic userTopic, CancellationToken cancellationToken)
        {
            userTopic.UserGuid = UserGuid();
            var topicGuid = await this.topicService.CreateTopic(userTopic, cancellationToken);
            return Ok(topicGuid);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTopic([FromBody] UserTopic userTopic, CancellationToken cancellationToken)
        {
            var topic = await this.topicService.UpdateTopic(userTopic, cancellationToken);
            return Ok(topic);
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveTopic(Guid guid, bool withWords, CancellationToken cancellationToken)
        {
            await this.topicService.RemoveTopic(UserGuid() ,guid, withWords, cancellationToken);
            return Ok();
        }
    }
}
