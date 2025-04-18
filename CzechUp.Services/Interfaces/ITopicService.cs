using CzechUp.EF.Models;

namespace CzechUp.Services.Interfaces
{
    public interface ITopicService
    {
        public Task<List<UserTopic>> GetTopics(Guid userGuid, CancellationToken cancellationToken);

        public Task<UserTopic> GetTopic(Guid topicGuid, CancellationToken cancellationToken);

        public Task<Guid> CreateTopic(UserTopic topic, CancellationToken cancellationToken);

        public Task<UserTopic> UpdateTopic(UserTopic topic, CancellationToken cancellationToken);

        public Task RemoveTopic(Guid userGuid, Guid topicGuid, bool withWords, CancellationToken cancellationToken);
    }
}
