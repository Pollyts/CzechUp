using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CzechUp.EF.Models
{
    public class GeneralWordTranslation
    {
        [Key]
        public Guid Guid { get; set; }
        public string Translation { get; set; }

        [ForeignKey("GeneralOriginalWord")]
        public Guid GeneralOriginalWordGuid { get; set; }

        [ForeignKey("Language")]
        public Guid LanguageGuid { get; set; }
        public GeneralOriginalWord GeneralOriginalWord { get; set; }
        public Language Language { get; set; }
    }
}
