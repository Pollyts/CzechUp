using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CzechUp.EF.Models
{
    public class User
    {
        [Key]
        public Guid Guid { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        [ForeignKey("TranslatedLanguage")]
        public Guid TranslatedLanguageGuid {  get; set; }
        public Language TranslatedLanguage { get; set; }

        [ForeignKey("RequiredLanguageLevel")]
        public Guid RequiredLanguageLevelGuid { get; set; }
        public LanguageLevel RequiredLanguageLevel { get; set; }
    }
}
