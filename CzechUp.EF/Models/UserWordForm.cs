using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CzechUp.EF.Models
{
    public class UserWordForm
    {
        [Key]
        public Guid Guid { get; set; }
        public string WordForm {  get; set; }
        public string Tag { get; set; }

        [ForeignKey("UserOriginalWord")]
        public Guid UserOriginalWordGuid { get; set; }
        public UserOriginalWord UserOriginalWord { get; set; }        
    }
}
