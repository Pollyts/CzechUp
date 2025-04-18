using CzechUp.EF;
using CzechUp.EF.Models;
using CzechUp.Services.DTOs;
using CzechUp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CzechUp.Services.Services
{
    public class WordService : BaseService<UserOriginalWord>, IWordService
    {
        private readonly DatabaseContext _databaseContext;
        public WordService(DatabaseContext databaseContext): base(databaseContext) {
            _databaseContext = databaseContext;
        }
        public async Task<List<UserOriginalWord>> GetWords(Guid userGuid, CancellationToken cancellationToken)
        {
            return await GetQuery().Where(w => w.UserGuid == userGuid).ToListAsync(cancellationToken);
        }

        public async Task<WordDto> GetWord(Guid userGuid, Guid wordGuid, CancellationToken cancellationToken)
        {
            var word = await GetQuery()
                .Include(w => w.LanguageLevel)
                .Include(w => w.UserTopic)
                .Where(w => w.UserGuid == userGuid && w.Guid == wordGuid).FirstAsync(cancellationToken);

            var wordTranslations = await _databaseContext.UserWordTranslations
                .Where(t => t.UserOriginalWordGuid == wordGuid)
                .ToListAsync(cancellationToken);

            var wordExamples = await _databaseContext.UserWordExamples
                .Where(t => t.UserOriginalWordGuid == wordGuid)
                .ToListAsync(cancellationToken);

            var wordForms = await _databaseContext.UserWordForms
                .Where(t => t.UserOriginalWordGuid == wordGuid)
                .ToListAsync(cancellationToken);

            return new WordDto()
            {
                Guid = wordGuid,
                Word = word.Word,
                Topic = word.UserTopic!.Name,
                LanguageLevel = word.LanguageLevel!.Name,
                Translations = new List<string>(wordTranslations.Select(t => t.Translation)),
                WordExamples = new List<WordExampleDto>(wordExamples.Select(e => new WordExampleDto() { Guid = e.Guid, OriginalExample = e.OriginalExample, TranslatedExample = e.TranslatedExample }))
            };
        }
    }
}
