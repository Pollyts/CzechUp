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
        string OriginalExample { get; set; }
        string TranslatedExample { get; set; }
    }
}
