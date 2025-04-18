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

        [HttpGet("word")]
        public async Task<IActionResult> GetWord(Guid wordGuid, CancellationToken cancellationToken)
        {
            var word = await this.wordService.GetWord(UserGuid(), wordGuid, cancellationToken);
            return Ok(word);
        }
    }
}
