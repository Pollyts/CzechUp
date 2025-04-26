using CzechUp.EF;
using CzechUp.EF.Models;
using CzechUp.Services.DTOs;
using CzechUp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CzechUp.WebApi.Controllers
{
    [ApiController]
    public class ExerciseController : BaseController
    {
        //private readonly ILogger<WeatherForecastController> _logger;
        private IExerciseService exerciseService;

        public ExerciseController(/*ILogger<WeatherForecastController> logger, */IExerciseService exerciseService)
        {
            this.exerciseService = exerciseService;
        }

        [HttpGet()]
        public async Task<IActionResult> GetExercise(Guid guid, CancellationToken cancellationToken)
        {
            var exercise = await this.exerciseService.GetUserExercise(UserGuid(), guid, cancellationToken);
            return Ok(exercise);
        }

        [HttpPost("withFilter")]
        public async Task<IActionResult> GetExercisesWithFilter([FromBody] FilterExerciseDto filter, CancellationToken cancellationToken)
        {
            var exercises = await this.exerciseService.GetUserExercisesWithFilter(UserGuid(), filter, cancellationToken);
            return Ok(exercises);
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateExercises([FromBody] ExerciseGeneratorDto parameters, CancellationToken cancellationToken)
        {
            var exercises = await this.exerciseService.GenerateExercises(UserGuid(), parameters, cancellationToken);
            return Ok(exercises);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateExercises([FromBody] ExerciseResultDto parameters, CancellationToken cancellationToken)
        {
            var exercises = await this.exerciseService.UpdateExercise(UserGuid(), parameters, cancellationToken);
            return Ok(exercises);
        }
    }
}
