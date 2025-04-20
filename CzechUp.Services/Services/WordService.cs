using CzechUp.EF;
using CzechUp.EF.Models;
using CzechUp.Helper.ApiHelper;
using CzechUp.Services.DTOs;
using CzechUp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

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
            return await GetQuery().Where(w => w.UserGuid == userGuid).Take(20).ToListAsync(cancellationToken);
        }

        public async Task<WordDto> GetWord(Guid userGuid, Guid wordGuid, CancellationToken cancellationToken)
        {
            var word = await GetQuery()
                .Include(w => w.LanguageLevel)
                .Include(w => w.UserTopic)
                .Include(w => w.UserTags)
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
                Tags = word.UserTags.Select(ut=>ut.Name).ToList(),
                Topic = word.UserTopicGuid.HasValue? word.UserTopic.Name: string.Empty,
                LanguageLevel = word.LanguageLevelGuid.HasValue? word.LanguageLevel.Name: string.Empty,
                Translations = new List<string>(wordTranslations.Select(t => t.Translation)),
                WordExamples = new List<WordExampleDto>(wordExamples.Select(e => new WordExampleDto() { Guid = e.Guid, OriginalExample = e.OriginalExample, TranslatedExample = e.TranslatedExample })),
                WordForms = new List<WordFormDto>(wordForms.Select(f=> new WordFormDto() { Form = f.WordForm, Tag = f.Tag, Guid = f.Guid}))
            };
        }

        public async Task<SearchedWordDto> SearchWord(string word, Guid userGuid, CancellationToken cancellationToken)
        {
            //try to search word in db
            var wordFromDb = _databaseContext.UserOriginalWords.Where(w => w.Word == word).FirstOrDefault();
            if(wordFromDb != null)
            {
                return new SearchedWordDto()
                {
                    Word = new WordDto()
                    {
                        Guid = wordFromDb.Guid
                    },
                    CanAddToDb = false
                };
            }

            //try to find word from SeznamSlovnik

            var wordDto = new WordDto();
            var user = _databaseContext.Users.Include(u=>u.TranslatedLanguage).FirstOrDefault(u => u.Guid == userGuid);
            using HttpClient wordTranslateClient = new();
            using HttpClient wordFormClient = new();
            using HttpClient translateSentenceClient = new();
            string apiKey = "";
            translateSentenceClient.DefaultRequestHeaders.Add("Authorization", $"DeepL-Auth-Key {apiKey}");

            try
            {
                var mainWords = word.Split(' ');
                if (mainWords.Count() == 1 || mainWords.Count() == 2 && (mainWords[1] == "se" || mainWords[1] == "si"))
                {
                    //translate word
                    var translateWordResult = await TranslateWordHelper.TranslateWord(wordTranslateClient, word, user.TranslatedLanguage.Name.ToLower());

                    if (translateWordResult != null && translateWordResult.MainInfo.Count > 0)
                    {
                        //search word in db
                        wordFromDb = _databaseContext.UserOriginalWords.Where(w => w.Word == translateWordResult.MainInfo.First().Head.FoundedWord).FirstOrDefault();
                        if (wordFromDb != null)
                        {
                            return new SearchedWordDto()
                            {
                                Word = await GetWord(userGuid, wordFromDb.Guid, cancellationToken),
                                CanAddToDb = false
                            };
                        }
                        wordDto.Word = translateWordResult.MainInfo.First().Head.FoundedWord;
                        wordDto.Translations = new List<string>();
                        foreach (var meaning in translateWordResult.MainInfo.First().Translations.First().Meanings)
                        {
                            var translation = string.Join("", meaning.Translations.First());
                            wordDto.Translations.Add(Regex.Replace(translation, "\\<.*?>", ""));
                        }
                        wordDto.WordExamples = new List<WordExampleDto>();
                        foreach (var translationExample in translateWordResult.ExampleSentences)
                        {
                            wordDto.WordExamples.Add(new WordExampleDto()
                            {
                                OriginalExample = Regex.Replace(translationExample.OriginalSentence, "\\<.*?>", ""),
                                TranslatedExample = translationExample.TranslatedSentence,

                            });
                        }
                        wordDto.WordForms = new List<WordFormDto>();
                        var forms = await WordFormHelper.GetWordForms(wordFormClient, translateWordResult.MainInfo.First().Head.FoundedWord);

                        foreach (var form in forms)
                        {
                            wordDto.WordForms.Add(new WordFormDto()
                            {
                                Form = form.Word,
                                Tag = form.Tag,
                            });
                        }
                    }
                    else
                    {
                        var translateSentenceResult = await TranslateSentenceHelper.TranslateSentence(translateSentenceClient, word, user.TranslatedLanguage.Name);
                        wordDto.Translations = [translateSentenceResult.TranslationTexts.First().Text];
                        wordDto.WordForms = new List<WordFormDto>();
                        var forms = await WordFormHelper.GetWordForms(wordFormClient, word);

                        foreach (var form in forms)
                        {
                            wordDto.WordForms.Add(new WordFormDto()
                            {
                                Form = form.Word,
                                Tag = form.Tag,
                            });
                        }
                    }
                }
                else
                {
                    //translate all sentence
                    var translateSentenceResult = await TranslateSentenceHelper.TranslateSentence(translateSentenceClient, word, user.TranslatedLanguage.Name);

                    wordDto.Translations.Add(translateSentenceResult.TranslationTexts.First().Text);
                    wordDto.Word = word;
                }
            }
            catch (Exception ex)
            {                
            }
            return new SearchedWordDto()
            {                
                Word = wordDto,
                CanAddToDb = true,
            };
        }

        public async Task<Guid> CreateWord(WordDto wordDto, Guid userGuid, CancellationToken cancellationToken)
        {
            var word = new UserOriginalWord()
            {
                LanguageLevelGuid = _databaseContext.LanguageLevels.Where(l => l.Name == wordDto.LanguageLevel).Select<LanguageLevel, Guid?>(l => l.Guid).FirstOrDefault(),
                UserGuid = userGuid,
                UserTopicGuid = _databaseContext.UserTopics.Where(t => t.Name == wordDto.Topic).Select<UserTopic, Guid?>(l => l.Guid).FirstOrDefault(),
                Word = wordDto.Word,
                UserTags = new List<UserTag>()
            };

            foreach(var tag in wordDto.Tags)
            {
                var dbTag = _databaseContext.UserTags.Where(t => t.UserGuid == userGuid && t.Name == tag).FirstOrDefault();
                if(dbTag != null)
                {
                    word.UserTags.Add(dbTag);
                }                
            }

            _databaseContext.Add(word);
            await _databaseContext.SaveChangesAsync(cancellationToken);

            foreach(var translation in wordDto.Translations)
            {
                _databaseContext.Add(new UserWordTranslation()
                {
                    Translation = translation,
                    UserGuid = userGuid,
                    UserOriginalWordGuid = word.Guid
                });
            }

            foreach (var example in wordDto.WordExamples)
            {
                _databaseContext.Add(new UserWordExample()
                {
                    OriginalExample = example.OriginalExample,
                    TranslatedExample = example.TranslatedExample,
                    UserOriginalWordGuid = word.Guid,
                });
            }

            foreach (var wordFrom in wordDto.WordForms)
            {
                _databaseContext.Add(new UserWordForm()
                {
                    WordForm = wordFrom.Form,
                    Tag = wordFrom.Tag,
                    UserOriginalWordGuid = word.Guid
                });
            }           

            await _databaseContext.SaveChangesAsync(cancellationToken);
            return word.Guid;
        }

        public async Task<Guid> UpdateWord(WordDto wordDto, Guid userGuid, CancellationToken cancellationToken)
        {
            var dbWord = _databaseContext.UserOriginalWords.Where(w => w.Guid == wordDto.Guid).Include(w => w.UserTags).FirstOrDefault();
            if (dbWord == null)
            {
                throw new Exception("Can not update word");
            }

            dbWord.LanguageLevelGuid = _databaseContext.LanguageLevels.Where(l => l.Name == wordDto.LanguageLevel).Select<LanguageLevel, Guid?>(l => l.Guid).FirstOrDefault();
            dbWord.UserTopicGuid = _databaseContext.UserTopics.Where(t => t.Name == wordDto.Topic).Select<UserTopic, Guid?>(l => l.Guid).FirstOrDefault();
            dbWord.Word = wordDto.Word;

            var dbTranslations = _databaseContext.UserWordTranslations.Where(t => t.UserOriginalWordGuid == dbWord.Guid);
            _databaseContext.RemoveRange(dbTranslations);

            var dbExamples = _databaseContext.UserWordExamples.Where(t => t.UserOriginalWordGuid == dbWord.Guid);
            _databaseContext.RemoveRange(dbExamples);

            dbWord.UserTags.Clear();

            foreach (var tag in wordDto.Tags)
            {
                var dbTag = _databaseContext.UserTags.Where(t => t.UserGuid == userGuid && t.Name == tag).FirstOrDefault();
                if (dbTag != null)
                {
                    dbWord.UserTags.Add(dbTag);
                }
            }

            await _databaseContext.SaveChangesAsync();

            foreach (var translation in wordDto.Translations)
            {
                _databaseContext.Add(new UserWordTranslation()
                {
                    Translation = translation,
                    UserGuid = userGuid,
                    UserOriginalWordGuid = dbWord.Guid
                });
            }

            foreach (var example in wordDto.WordExamples)
            {
                _databaseContext.Add(new UserWordExample()
                {
                    OriginalExample = example.OriginalExample,
                    TranslatedExample = example.TranslatedExample,
                    UserOriginalWordGuid = dbWord.Guid,
                });
            }

            

            await _databaseContext.SaveChangesAsync(cancellationToken);
            return wordDto.Guid;
        }

        public async Task DeleteWord(Guid wordGuid, Guid userGuid, CancellationToken cancellationToken)
        {
            var dbWord = GetByGuid(wordGuid);
            if (dbWord == null)
            {
                throw new Exception("Can not delete word");
            }

            var dbTranslations = _databaseContext.UserWordTranslations.Where(t => t.UserOriginalWordGuid == dbWord.Guid);
            _databaseContext.RemoveRange(dbTranslations);

            var dbExamples = _databaseContext.UserWordExamples.Where(t => t.UserOriginalWordGuid == dbWord.Guid);
            _databaseContext.RemoveRange(dbExamples);

            var dbForms = _databaseContext.UserWordForms.Where(t => t.UserOriginalWordGuid == dbWord.Guid);
            _databaseContext.RemoveRange(dbForms);

            _databaseContext.Remove(dbWord);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task<List<UserOriginalWord>> GetWordsWithFilter(Guid userGuid, FilterWordDto filter, CancellationToken cancellationToken)
        {
            return await GetQuery()
                .Where(w => w.UserGuid == userGuid)
                .Where(w=>filter.Topics.Count == 0 || (w.UserTopicGuid.HasValue && filter.Topics.Contains(w.UserTopicGuid.Value)))
                .Where(w => filter.Tags.Count == 0 || w.UserTags.Any(t=> filter.Tags.Contains(t.Guid)))
                .Take(20)
                .ToListAsync(cancellationToken);
        }
    }
}
