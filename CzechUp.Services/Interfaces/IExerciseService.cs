using CzechUp.EF.Models;
using CzechUp.Services.DTOs;

namespace CzechUp.Services.Interfaces
{
    public interface IExerciseService
    {
        Task<List<ExerciseResultDto>> GetUserExercisesWithFilter(Guid userGuid, FilterExerciseDto filter, CancellationToken cancellationToken);
        Task<List<ExerciseDto>> GenerateExercises(Guid userGuid, ExerciseGeneratorDto parameters, CancellationToken cancellationToken);
        Task<Guid> UpdateExercise(Guid userGuid, ExerciseResultDto exerciseDto, CancellationToken cancellationToken);
        Task<ExerciseDto> GetUserExercise(Guid userGuid, Guid guid, CancellationToken cancellationToken);
    }
}
