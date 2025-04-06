using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CzechUp.EF.Models
{
    public class UserTag
    {
        [Key]
        public Guid Guid { get; set; }
        public string Name { get; set; }

        [ForeignKey("User")]
        public Guid UserGuid { get; set; }
        public User User { get; set; }
        public List<TagType> TagTypes { get; set; }

    }
}
