using CzechUp.EF;
using CzechUp.EF.Models;
using CzechUp.Services.DTOs;
using CzechUp.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;
namespace CzechUp.Services.Services
{
    public class UserService : IUserService
    {
        private readonly PasswordHasher<User> passwordHasher = new PasswordHasher<User>();
        private readonly DatabaseContext _databaseContext;

        public UserService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public Guid CreateUser(RegistrationRequestDto request)
        {
            var createUser = new User()
            {
                Email = request.Email,
                Password = HashPassword(request.Password),
                RequiredLanguageLevelGuid = request.RequiredLanguageLevelGuid,
                TranslatedLanguageGuid = request.OriginLanguageGuid
            };

            _databaseContext.Users.Add(createUser);
            _databaseContext.SaveChanges();

            //add topics from general topic dictionary
            var generalTopics = _databaseContext.GeneralTopics.ToList();
            foreach (var generalTopic in generalTopics)
            {
                _databaseContext.UserTopics.Add(new UserTopic()
                {
                    GeneralTopicGuid = generalTopic.Guid,
                    Name = generalTopic.Name,
                    UserGuid = createUser.Guid
                });
            }

            _databaseContext.SaveChanges();

            //add words from general word dictionary

            var userTopics = _databaseContext.UserTopics.Where(ut => ut.UserGuid == createUser.Guid).ToList();
            var allLanguageLevels = _databaseContext.LanguageLevels.OrderBy(l => l.Name).ToList();
            var requiredLanguageLevel = allLanguageLevels.Where(x => x.Guid == request.RequiredLanguageLevelGuid).Single();
            List<Guid> requiredLanguageLevelGuids = new List<Guid>()
            {
                allLanguageLevels.First().Guid//A1
            };

            if (requiredLanguageLevel.Name == "A2" || requiredLanguageLevel.Name == "B1" || requiredLanguageLevel.Name == "B2")
            {
                requiredLanguageLevelGuids.Add(allLanguageLevels.Where(l => l.Name == "A2").Single().Guid);
            }

            if (requiredLanguageLevel.Name == "B1" || requiredLanguageLevel.Name == "B2")
            {
                requiredLanguageLevelGuids.Add(allLanguageLevels.Where(l => l.Name == "B1").Single().Guid);
            }

            if (requiredLanguageLevel.Name == "B2")
            {
                requiredLanguageLevelGuids.Add(allLanguageLevels.Where(l => l.Name == "B2").Single().Guid);
            }

            foreach (var userTopic in userTopics)
            {
                var wordsForLevelAndTopic = _databaseContext.GeneralOriginalWords.Where(w => requiredLanguageLevelGuids.Contains(w.LanguageLevelGuid) && w.GeneralTopics.Any(t=>t.Guid == userTopic.GeneralTopicGuid)).ToList();

                foreach (var w in wordsForLevelAndTopic)
                {
                    var existingWord = _databaseContext.UserOriginalWords.Include(w => w.UserTopics).FirstOrDefault(w => w.Word == w.Word);
                    if (existingWord != null)
                    {
                        existingWord.UserTopics.Add(userTopic);
                    }
                    var userWord = new UserOriginalWord()
                    {
                        LanguageLevelGuid = w.LanguageLevelGuid,
                        Word = w.Word,
                        UserGuid = createUser.Guid,
                        GeneralOriginalWordGuid = w.Guid,
                        UserTopics = new List<UserTopic>()
                    };
                    userWord.UserTopics.Add(userTopic);
                    _databaseContext.UserOriginalWords.Add(userWord);
                }
            }

            _databaseContext.SaveChanges();

            //add translations, word forms and examples from general word translations
            var userWords = _databaseContext.UserOriginalWords.Where(w => w.UserGuid == createUser.Guid).ToList();

            foreach (var userWord in userWords)
            {
                var translations = _databaseContext.GeneralWordTranslations.Where(t => t.LanguageGuid == request.OriginLanguageGuid && t.GeneralOriginalWordGuid == userWord.GeneralOriginalWordGuid).ToList();

                foreach (var translation in translations)
                {
                    _databaseContext.UserWordTranslations.Add(new UserWordTranslation()
                    {
                        UserGuid = createUser.Guid,
                        Translation = translation.Translation,
                        UserOriginalWordGuid = userWord.Guid
                    });
                }

                var forms = _databaseContext.GeneralWordForms.Where(f => f.GeneralOriginalWordGuid == userWord.GeneralOriginalWordGuid).ToList();

                foreach (var form in forms)
                {
                    _databaseContext.UserWordForms.Add(new UserWordForm()
                    {
                        UserOriginalWordGuid = userWord.Guid,
                        Tag = form.Tag,
                        WordForm = form.WordForm
                    });
                }

                var examples = _databaseContext.GeneralWordExamples.Where(f => f.GeneralOriginalWordGuid == userWord.GeneralOriginalWordGuid && f.LanguageGuid == request.OriginLanguageGuid).ToList();

                foreach (var example in examples)
                {
                    _databaseContext.UserWordExamples.Add(new UserWordExample()
                    {
                        OriginalExample = example.OriginalExample,
                        TranslatedExample = example.TranslatedExample,
                        UserOriginalWordGuid = userWord.Guid
                    });
                }
            }

            //add empty rule notes
            var rules = _databaseContext.Rules.ToList();
            foreach (var rule in rules)
            {
                _databaseContext.UserRuleNotes.Add(new UserRuleNote()
                {
                    Note = string.Empty,
                    RuleGuid = rule.Guid,
                    UserGuid = createUser.Guid
                });
            }
            _databaseContext.SaveChanges();

            return createUser.Guid;
        }

        public User? Login(LoginRequestDto loginRequest)
        {
            var user = _databaseContext.Users.FirstOrDefault(u => u.Email == loginRequest.Email);
            if (user != null && VerifyPassword(user.Password, loginRequest.Password))
            {
                return user;
            }
            else
            {
                return null;
            }
        }

        public bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            return passwordHasher.VerifyHashedPassword(null, hashedPassword, providedPassword) == PasswordVerificationResult.Success;
        }

        private string HashPassword(string password)
        {
            return passwordHasher.HashPassword(null, password);
        }
    }
}
