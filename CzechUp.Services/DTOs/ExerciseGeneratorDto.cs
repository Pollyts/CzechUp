using CzechUp.EF.Models;

namespace CzechUp.Services.DTOs
{
    public class ExerciseGeneratorDto
    {
        public List<Guid> Topics { get; set; }
        public List<ExerciseType> ExerciseTypes { get; set; }
        public List<Guid> LanguageLevels { get; set; }
        public int Count { get; set; }
        public bool OnlyNew { get; set; }
        public List<Guid> Tags { get; set; }
    }
}
