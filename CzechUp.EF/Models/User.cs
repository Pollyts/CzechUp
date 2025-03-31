using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CzechUp.EF.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int TranslatedLanguageId {  get; set; }
        public Language TranslatedLanguage { get; set; }
        public int RequiredLanguageLevelId { get; set; }
        public LanguageLevel RequiredLanguageLevel { get; set; }
    }
}
