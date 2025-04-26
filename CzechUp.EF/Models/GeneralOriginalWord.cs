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
    public class GeneralOriginalWord : IDbEntity
    {
        [Key]
        public Guid Guid { get; set; }
        public string Word { get; set; }

        [ForeignKey("LanguageLevel")]
        public Guid LanguageLevelGuid { get; set; }
        public List<GeneralTopic> GeneralTopics { get; set; }
        public LanguageLevel LanguageLevel { get; set; }
    }
}
