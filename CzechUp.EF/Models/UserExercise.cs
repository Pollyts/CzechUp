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
    public class UserExercise : IDbEntity
    {
        [Key]
        public Guid Guid { get; set; }

        [ForeignKey("GeneralExercise")]
        public Guid GeneralExerciseGuid { get; set; }

        [ForeignKey("User")]
        public Guid UserGuid { get; set; }
        public int CorrectAnswerCount { get; set; }
        public int WrongAnswerCount { get; set; }
        public User User { get; set; }
        public GeneralExercise GeneralExercise { get; set; }
        public List<UserTag> UserTags { get; set; }
    }
}
