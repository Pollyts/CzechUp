using CzechUp.EF;
using CzechUp.EF.Models;
using CzechUp.Services.DTOs;
using CzechUp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CzechUp.Services.Services
{
    public class ExerciseService : BaseService<UserExercise>, IExerciseService
    {
        private readonly DatabaseContext _databaseContext;

        public ExerciseService(DatabaseContext databaseContext) : base(databaseContext)
        {
            _databaseContext = databaseContext;
        }
        public async Task<List<ExerciseResultDto>> GetUserExercisesWithFilter(Guid userGuid, FilterExerciseDto filter, CancellationToken cancellationToken)
        {
            return await GetQuery()
                .Include(e=>e.GeneralExercise)
                .Where(w => w.UserGuid == userGuid)
                .Where(w => filter.Topics.Count == 0 || filter.Topics.Contains(w.GeneralExercise.GeneralTopic.Guid))
                .Where(w => filter.Tags.Count == 0 || w.UserTags.Any(t => filter.Tags.Contains(t.Guid)))
                .Where(w => filter.CompleteResults.Count == 0 || filter.CompleteResults.Any(r=>r == CompleteResult.WithMistakes) && w.CorrectAnswerCount > 0 || filter.CompleteResults.Any(r => r == CompleteResult.WithMistakes) && w.WrongAnswerCount > 0 && w.CorrectAnswerCount == 0)
                .Where(w => filter.ExerciseTypes.Count == 0 || filter.ExerciseTypes.Contains(w.GeneralExercise.ExerciseType))
                .Where(w => filter.LanguageLevels.Count == 0 || filter.LanguageLevels.Contains(w.GeneralExercise.LanguageLevel.Guid))
                .Take(20)
                .Select(e=> new ExerciseResultDto()
                {
                    Guid = e.Guid,
                    ExerciseType = e.GeneralExercise.ExerciseType,
                    LastUsed = e.LastUsed,
                    Result = e.CorrectAnswerCount  > 0
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<ExerciseDto> GetUserExercise(Guid userGuid, Guid guid, CancellationToken cancellationToken)
        {
            return await GetQuery().Where(e=>e.UserGuid == userGuid && e.Guid == guid).Include(e=>e.GeneralExercise).Select(e=> new ExerciseDto()
            {
                Guid = e.Guid,
                ExerciseType = e.GeneralExercise.ExerciseType,
                Answer = e.GeneralExercise.Answer,
                AnswerOptions = e.GeneralExercise.AnswerOptions,
                Question = e.GeneralExercise.Question,
            }).FirstAsync();
        }

        public async Task<List<ExerciseDto>> GenerateExercises(Guid userGuid, ExerciseGeneratorDto parameters, CancellationToken cancellationToken)
        {
            var query = _databaseContext.GeneralExercises
                .Where(e => parameters.Topics.Count == 0 || e.GeneralTopicGuid.HasValue && parameters.Topics.Contains(e.GeneralTopicGuid.Value))
                .Where(e => parameters.ExerciseTypes.Count == 0 || parameters.ExerciseTypes.Contains(e.ExerciseType))
                .Where(e => parameters.LanguageLevels.Count == 0 || parameters.LanguageLevels.Contains(e.LanguageLevelGuid));

            if (parameters.OnlyNew)
            {
                var userTasks = _databaseContext.UserExercises.Where(e=>e.UserGuid == userGuid).Select(e=>e.GeneralExerciseGuid);
                query = query.Where(e => !userTasks.Contains(e.Guid));
            }

            var list = query.Take(parameters.Count).ToList();
            var result = new List<ExerciseDto>();

            foreach (var exerciseDto in list)
            {
                var userExercise = new UserExercise()
                {
                    CorrectAnswerCount = 0,
                    WrongAnswerCount = 0,
                    GeneralExerciseGuid = exerciseDto.Guid,
                    LastUsed = DateTime.UtcNow,
                    UserGuid = userGuid,
                    UserTags = _databaseContext.UserTags.Where(t => t.UserGuid == userGuid && parameters.Tags.Contains(t.Guid)).ToList(),
                };
                _databaseContext.Add(userExercise);
                _databaseContext.SaveChanges();

                result.Add(new ExerciseDto()
                {
                    Guid = userExercise.Guid,
                    Answer = exerciseDto.Answer,
                    AnswerOptions = exerciseDto.AnswerOptions,
                    Question = exerciseDto.Question,
                });
            }

            return result.ToList();
        }

        public async Task<Guid> UpdateExercise(Guid userGuid, ExerciseResultDto exerciseDto, CancellationToken cancellationToken)
        {
            var dbExercise = _databaseContext.UserExercises.Where(e => e.Guid == exerciseDto.Guid && e.UserGuid == userGuid).FirstOrDefault();
            if (dbExercise == null)
            {
                throw new Exception("Can not update exercise");
            }

            dbExercise.LastUsed = DateTime.UtcNow;
            if (exerciseDto.Result)
            {
                dbExercise.CorrectAnswerCount++; 
            }
            else
            {
                dbExercise.WrongAnswerCount++;
            }
            _databaseContext.SaveChanges();
            return dbExercise.Guid;
        }
    }
}
