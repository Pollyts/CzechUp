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
    public class UserWordTranslation : IDbEntity
    {
        [Key]
        public Guid Guid { get; set; }
        public string Translation { get; set; }

        [ForeignKey("UserOriginalWord")]
        public Guid UserOriginalWordGuid { get; set; }

        [ForeignKey("UserGuid")]
        public Guid UserGuid {  get; set; }
        public User User { get; set; }     
        public UserOriginalWord UserOriginalWord { get; set; }       
        
    }
}
