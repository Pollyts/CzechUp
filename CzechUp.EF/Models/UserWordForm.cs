using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CzechUp.EF.Models
{
    public class UserWordForm
    {
        public int Id { get; set; }
        public string WordForm {  get; set; }
        public string Tag { get; set; }
        public int UserWordId { get; set; }
        public UserOriginalWord OriginalWord { get; set; }        
    }
}
