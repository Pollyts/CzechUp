using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CzechUp.EF.Models
{
    public class UserRuleNote
    {
        public int Id { get; set; }
        public int RuleId { get; set; }
        public int UserId { get; set; }
        public string Note { get; set; }
        public Rule Rule { get; set; }
        public User User { get; set; }
    }
}
