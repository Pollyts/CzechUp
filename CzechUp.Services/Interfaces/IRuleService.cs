using CzechUp.EF.Models;

namespace CzechUp.Services.Interfaces
{
    public interface IRuleService
    {
        Task<List<UserOriginalWord>> GetWords(Guid userGuid, CancellationToken cancellationToken);
    }
}
