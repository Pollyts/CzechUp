using CzechUp.EF.Models.Absract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CzechUp.EF.Models
{
    public class GeneralExercise: IDbEntity
    {
        [Key]
        public Guid Guid { get; set; }
        public ExerciseType ExerciseType { get; set; }
        public string Question { get; set; }
        public string AnswerOptions { get; set; } //suggested answer options
        public string Answer { get; set; }

        [ForeignKey("GeneralTopic")]
        public Guid? GeneralTopicGuid { get; set; }

        [ForeignKey("GeneralOriginalWord")]
        public Guid? GeneralOriginalWordGuid { get; set; }

        [ForeignKey("TranslatedLanguage")]
        public Guid? TranslatedLanguageGuid { get; set; }

        [ForeignKey("LanguageLevel")]
        public Guid LanguageLevelGuid { get; set; }        
        public GeneralTopic? GeneralTopic { get; set; }
        public GeneralOriginalWord? GeneralOriginalWord { get; set; }        
        public Language? TranslatedLanguage { get; set; }
        public LanguageLevel LanguageLevel { get; set; }
    }

    public enum ExerciseType
    {
        InsertWordInRightForm, // Заполнение пропуска подходящим словом в правильной форме. 
        CreateSentence, //Составление предложений с новыми словами
        InsertWordToText, //	Заполнение пропусков в тексте словами из предложенных вариантов
        MatchingWordAndItsTranslate, //	Выбрать правильный перевод слова из предложенных
        WriteCzechWord, // Написать перевод слова
    }
}
