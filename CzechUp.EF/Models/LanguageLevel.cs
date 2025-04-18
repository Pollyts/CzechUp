using CzechUp.EF.Models.Absract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CzechUp.EF.Models
{
    public class LanguageLevel : IDbEntity
    {
        [Key]
        public Guid Guid { get; set; }
        public required string Name { get; set; }
    }
}
