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
    public class GeneralWordExample : IDbEntity
    {
        [Key]
        public Guid Guid { get; set; }

        [ForeignKey("GeneralOriginalWord")]
        public Guid GeneralOriginalWordGuid { get; set; }

        [ForeignKey("Language")]
        public Guid LanguageGuid { get; set; }
        public string OriginalExample { get; set; }
        public string TranslatedExample { get; set; }
        public GeneralOriginalWord GeneralOriginalWord { get; set; }
        public Language Language { get; set; }
    }
}
