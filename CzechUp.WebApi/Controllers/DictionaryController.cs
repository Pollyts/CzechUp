using CzechUp.EF;
using CzechUp.EF.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CzechUp.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DictionaryController : BaseController
    {
        //private readonly ILogger<WeatherForecastController> _logger;
        private readonly DatabaseContext _databaseContext;

        public DictionaryController(/*ILogger<WeatherForecastController> logger, */DatabaseContext dbContext)
        {
            _databaseContext = dbContext;
        }

        [HttpGet("LanguageLevels")]
        public List<LanguageLevel> GetLanguageLevelList()
        {
            return _databaseContext.LanguageLevels.AsQueryable().ToList();
        }

        [HttpGet("Languages")]
        public List<Language> GetLanguageList()
        {
            return _databaseContext.Languages.AsQueryable().ToList();
        }
    }
}
