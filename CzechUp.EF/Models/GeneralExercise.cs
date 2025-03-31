using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CzechUp.EF.Models
{
    public class GeneralExercise
    {
        public int Id { get; set; }
        public ExerciseType ExerciseType { get; set; }
        public string Question { get; set; }
        public string AnswerOptions { get; set; } //suggested answer options
        public string Answer { get; set; }
    }

    public enum ExerciseType
    {
        MatchingWordAndItsTranslate, //Сопоставление слов и их переводов (matching)
        InsertWord, //Заполнение пропусков подходящими словами
        CreateSentence, //Составление предложений с новыми словами
        InsertWordInRightWorm, //Подстановка форм слов (например, спряжение глаголов)
        TransformSentence, //Трансформация предложений (например, активный → пассивный залог)
    }
}
