using CzechUp.Services.DTOs;
using CzechUp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CzechUp.WebApi.Controllers
{
    //[Authorize]
    public class WordController : BaseController
    {
        private IWordService wordService;
        public WordController(IWordService wordService) 
        { 
            this.wordService = wordService;
        }

        [HttpGet]
        public async Task<IActionResult> GetWords(CancellationToken cancellationToken)
        {            
            var words = await this.wordService.GetWords(UserGuid(), cancellationToken);
            return Ok(words);
        }

        [HttpPost("withFilter")]
        public async Task<IActionResult> GetWordsWithFilter([FromBody]FilterWordDto filter, CancellationToken cancellationToken)
        {
            var words = await this.wordService.GetWordsWithFilter(UserGuid(), filter, cancellationToken);
            return Ok(words);
        }

        [HttpGet("word")]
        public async Task<IActionResult> GetWord(Guid wordGuid, CancellationToken cancellationToken)
        {
            var word = await this.wordService.GetWord(UserGuid(), wordGuid, cancellationToken);
            return Ok(word);
        }

        [HttpGet("searchWord")]
        public async Task<IActionResult> SearchWord(string word, CancellationToken cancellationToken)
        {
            var foundedWord = await this.wordService.SearchWord(word, UserGuid(), cancellationToken);
            return Ok(foundedWord);
        }

        [HttpPost]
        public async Task<IActionResult> CreateWord([FromBody]WordDto word, CancellationToken cancellationToken)
        {
            var newWordGuid = await this.wordService.CreateWord(word, UserGuid(), cancellationToken);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateWord([FromBody] WordDto word, CancellationToken cancellationToken)
        {
            var newWordGuid = await this.wordService.UpdateWord(word, UserGuid(), cancellationToken);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveWord(Guid guid, CancellationToken cancellationToken)
        {
            await this.wordService.DeleteWord(guid, UserGuid(), cancellationToken);
            return Ok();
        }
    }
}
