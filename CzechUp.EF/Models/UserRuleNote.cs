using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CzechUp.EF.Models
{
    public class UserRuleNote
    {
        [Key]
        public Guid Guid { get; set; }

        [ForeignKey("Rule")]
        public Guid RuleGuid { get; set; }

        [ForeignKey("User")]
        public Guid UserGuid { get; set; }
        public string Note { get; set; }
        public Rule Rule { get; set; }
        public User User { get; set; }
    }
}
