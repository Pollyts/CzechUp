using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CzechUp.EF.Models
{
    public class GeneralWordForm
    {
        public int Id { get; set; }
        public int WordNumber { get; set; }
        public string WordForm { get; set; }
        public string Tag { get; set; }
        public int OriginalWordId { get; set; }
        public GeneralOriginalWord OriginalWord { get; set; }        
    }
}
