using CzechUp.EF;
using CzechUp.EF.Models;
using CzechUp.Services.DTOs;
using CzechUp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CzechUp.Services.Services
{
    public class TopicService : BaseService<UserTopic>, ITopicService
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IWordService wordService;

        public TopicService(DatabaseContext databaseContext, IWordService wordService) : base(databaseContext)
        {
            _databaseContext = databaseContext;
            this.wordService = wordService;
        }
        public async Task<List<UserTopic>> GetTopics(Guid userGuid, CancellationToken cancellationToken)
        {
            return await GetQuery().Where(w => w.UserGuid == userGuid).ToListAsync(cancellationToken);
        }

        public async Task<UserTopic> GetTopic(Guid topicGuid, CancellationToken cancellationToken)
        {
            return GetByGuid(topicGuid);
        }

        public async Task<Guid> CreateTopic(UserTopic topic, CancellationToken cancellationToken)
        {            
            return Create(topic);
        }

        public async Task<UserTopic> UpdateTopic(UserTopic topic, CancellationToken cancellationToken)
        {
            var dbTopic = GetByGuid(topic.Guid);
            if (dbTopic == null || dbTopic.GeneralTopicGuid.HasValue)
            {
                throw new Exception("Can not update topic");
            }
            else
            {
                dbTopic.Name = topic.Name;
                _databaseContext.SaveChanges();
            }
            return dbTopic;
        }

        public async Task RemoveTopic(Guid userGuid, Guid topicGuid, bool withWords, CancellationToken cancellationToken)
        {
            var topic = GetByGuid(topicGuid);
            if (topic == null || topic.UserGuid != userGuid || topic.GeneralTopicGuid.HasValue)
            {
                throw new Exception("Can not delete topic");
            }
            else
            {
                var wordsFromTopic = await _databaseContext.UserOriginalWords.Where(w=>w.UserGuid == userGuid && w.UserTopicGuid == topicGuid).ToListAsync(cancellationToken);
                if (withWords)
                {
                    _databaseContext.UserOriginalWords.RemoveRange(wordsFromTopic);
                }
                else
                {
                    foreach (var word in wordsFromTopic)
                    {
                        word.UserTopicGuid = null;
                    }
                }
                _databaseContext.SaveChanges();
                _databaseContext.Remove(topic);
                _databaseContext.SaveChanges();
            }
        }
    }
}
