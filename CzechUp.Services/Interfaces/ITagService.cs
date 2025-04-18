using CzechUp.EF.Models;

namespace CzechUp.Services.Interfaces
{
    public interface ITagService
    {
        public Task<List<UserTag>> GetTags(Guid userGuid, CancellationToken cancellationToken);

        public Task<UserTag> GetTag(Guid tagGuid, CancellationToken cancellationToken);

        public Task<Guid> CreateTag(UserTag tag, CancellationToken cancellationToken);

        public Task<UserTag> UpdateTag(UserTag tag, CancellationToken cancellationToken);

        public Task RemoveTag(Guid userGuid, Guid tagGuid, CancellationToken cancellationToken);
    }
}
