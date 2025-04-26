using CzechUp.EF.Models;

namespace CzechUp.Services.DTOs
{
    public class FilterExerciseDto
    {
        public List<Guid> Tags { get; set; }
        public List<Guid> Topics { get; set; }
        public List<Guid> LanguageLevels { get; set; }
        public List<ExerciseType> ExerciseTypes { get; set; }
        public List<CompleteResult> CompleteResults { get; set; } //with mistakes, without mistakes
    }

    public enum CompleteResult
    {
        WithMistakes = 0,
        WithoutMistakes = 1
    }
}
