using CzechUp.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CzechUp.Services.DTOs
{
    public class ExerciseResultDto
    {
        public Guid Guid { get; set; }
        public DateTime LastUsed { get; set; }
        public ExerciseType ExerciseType { get; set; }
        public bool Result {  get; set; }
    }
}
