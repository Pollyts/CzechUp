using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CzechUp.EF.Models
{
    public class GeneralWordExample
    {
        public int Id { get; set; }
        public int GeneralOriginalWordId { get; set; }
        public string OriginalExample { get; set; }
        public string TranslatedExample { get; set; }
        public GeneralOriginalWord GeneralOriginalWord { get; set; }
    }
}
