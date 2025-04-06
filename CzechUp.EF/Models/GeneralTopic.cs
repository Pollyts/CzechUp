using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CzechUp.EF.Models
{
    public class GeneralTopic
    {
        [Key]
        public Guid Guid { get; set; }
        public string Name { get; set; }
    }
}
