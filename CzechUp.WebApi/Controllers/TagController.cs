using CzechUp.EF.Models;
using CzechUp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CzechUp.WebApi.Controllers
{
    public class TagController : BaseController
    {
        private readonly ITagService tagService;
        public TagController(ITagService tagService)
        {
            this.tagService = tagService;
        }
        [HttpGet]
        public async Task<IActionResult> GetTags(CancellationToken cancellationToken)
        {
            var tags = await this.tagService.GetTags(UserGuid(), cancellationToken);
            return Ok(tags);
        }

        [HttpGet("tag")]
        public async Task<IActionResult> GetTag(Guid guid, CancellationToken cancellationToken)
        {
            var tag = await this.tagService.GetTag(guid, cancellationToken);
            return Ok(tag);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTag([FromBody] UserTag userTag, CancellationToken cancellationToken)
        {
            userTag.UserGuid = UserGuid();
            var tagGuid = await this.tagService.CreateTag(userTag, cancellationToken);
            return Ok(tagGuid);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTag([FromBody] UserTag userTag, CancellationToken cancellationToken)
        {
            var tag = await this.tagService.UpdateTag(userTag, cancellationToken);
            return Ok(tag);
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveTag(Guid guid, CancellationToken cancellationToken)
        {
            await this.tagService.RemoveTag(UserGuid() ,guid, cancellationToken);
            return Ok();
        }
    }
}
