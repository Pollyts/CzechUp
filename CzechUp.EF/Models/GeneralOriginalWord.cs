﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CzechUp.EF.Models
{
    public class GeneralOriginalWord
    {
        [Key]
        public Guid Guid { get; set; }
        public string Word { get; set; }

        [ForeignKey("LanguageLevel")]
        public Guid LanguageLevelGuid { get; set; }

        [ForeignKey("GeneralTopic")]
        public Guid GeneralTopicGuid { get; set; }
        public GeneralTopic GeneralTopic { get; set; }
        public LanguageLevel LanguageLevel { get; set; }
    }
}
