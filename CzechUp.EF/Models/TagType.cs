using CzechUp.EF.Models.Absract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CzechUp.EF.Models
{
    public class TagType : IDbEntity
    {
        [Key]
        public Guid Guid { get; set; }
        public TagTypeEnum TagTypeEnum { get; set; }
    }

    public enum TagTypeEnum
    {
        Word = 0,
        Rule = 1,
        Topic = 2,
        Exercise =3
    }
}
