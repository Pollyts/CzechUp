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
    public class UserWordExample : IDbEntity
    {
        [Key]
        public Guid Guid { get; set; }

        [ForeignKey("UserOriginalWord")]
        public Guid UserOriginalWordGuid { get; set; }
        public UserOriginalWord UserOriginalWord { get; set; }
        public string OriginalExample { get; set; }
        public string TranslatedExample { get; set; }
    }
}
