using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CzechUp.EF.Models
{
    public class UserWordExample
    {        
        public int Id { get; set; }
        public int UserOriginalWordId { get; set; }
        public UserOriginalWord UserOriginalWord { get; set; }
        public string OriginalExample { get; set; }
        public string TranslatedExample { get; set; }
    }
}
