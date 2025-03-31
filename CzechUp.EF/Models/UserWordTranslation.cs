using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CzechUp.EF.Models
{
    public class UserWordTranslation
    {
        public int Id { get; set; }
        public string Translation { get; set; }
        public int UserOriginalWordId { get; set; }
        public int UserId {  get; set; }   
        public bool WasLearned { get; set; }
        public User User { get; set; }     
        public UserOriginalWord UserOriginalWord { get; set; }       
        
    }
}
