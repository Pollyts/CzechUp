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
    public class GeneralWordForm : IDbEntity
    {
        [Key]
        public Guid Guid { get; set; }
        public int WordNumber { get; set; }
        public string WordForm { get; set; }
        public string Tag { get; set; }

        [ForeignKey("GeneralOriginalWord")]
        public Guid GeneralOriginalWordGuid { get; set; }
        public GeneralOriginalWord GeneralOriginalWord { get; set; }        
    }
}
