using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CzechUp.EF.Models
{
    public class GeneralOriginalWord
    {
        public int Id { get; set; }
        public string Word { get; set; }
        public int LanguageLevelId { get; set; }
        public int GeneralTopicId { get; set; }
        public GeneralTopic GeneralTopic { get; set; }
        public LanguageLevel LanguageLevel { get; set; }
    }
}
