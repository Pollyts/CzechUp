using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CzechUp.EF.Models
{
    public class UserOriginalWord
    {
        public int Id { get; set; }
        public string Word { get; set; }
        public int? LanguageLevelId { get; set; }
        public int? UserTopicId { get; set; }
        public int? GeneralOriginalWordId { get; set; }
        public int UserId { get; set; }
        public UserTopic? UserTopic { get; set; }
        public LanguageLevel? LanguageLevel { get; set; }
        public User User { get; set; }
        public GeneralOriginalWord? GeneralOriginalWord { get; set; }
        public List<UserTag> UserTags { get; set; }
    }
}
