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
    public class UserOriginalWord : IDbEntity
    {
        [Key]
        public Guid Guid { get; set; }
        public string Word { get; set; }

        [ForeignKey("LanguageLevel")]
        public Guid? LanguageLevelGuid { get; set; }

        [ForeignKey("GeneralOriginalWord")]
        public Guid? GeneralOriginalWordGuid { get; set; }

        [ForeignKey("User")]
        public Guid UserGuid { get; set; }
        public List<UserTopic> UserTopics { get; set; }
        public LanguageLevel? LanguageLevel { get; set; }
        public User User { get; set; }
        public GeneralOriginalWord? GeneralOriginalWord { get; set; }
        public List<UserTag> UserTags { get; set; }
    }
}
