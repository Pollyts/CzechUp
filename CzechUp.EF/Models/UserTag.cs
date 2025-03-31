using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CzechUp.EF.Models
{
    public class UserTag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TagType TagType { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

    }

    public enum TagType
    {
        Word = 0,
        Rule = 1,
    }
}
