using CzechUp.EF.Models;

namespace CzechUp.Services.DTOs
{
    public class ExerciseDto
    {
        public Guid Guid { get; set; }
        public string Question { get; set; }
        public string AnswerOptions { get; set; }
        public string Answer { get; set; }
        public ExerciseType ExerciseType { get; set; }
    }
}
