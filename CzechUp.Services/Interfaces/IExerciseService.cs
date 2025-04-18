using CzechUp.EF.Models;

namespace CzechUp.Services.Interfaces
{
    public interface IExerciseService
    {
        Task<List<UserOriginalWord>> GetWords(Guid userGuid, CancellationToken cancellationToken);
    }
}
