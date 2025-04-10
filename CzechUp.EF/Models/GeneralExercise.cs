﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CzechUp.EF.Models
{
    public class GeneralExercise
    {
        [Key]
        public Guid Guid { get; set; }
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
        SelectAnswer, //Выбрать правильный ответ из предложенных
        WriteAnswer, //Заполнить пропуск в вопросе/ввести ответ на вопрос
        ChooseCategory, //Выбрать категорию
    }
}
