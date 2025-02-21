using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CzechUp.EF.Models
{
    public class UserWord
    {
        public int Id { get; set; }
        public int UserId {  get; set; }
        public User User { get; set; }
        public int? TopicId { get; set; }
        public UserTopic? Topic { get; set; }
        public int? GeneralWordId { get; set; }
        public GeneralWord GeneralWord { get; set; }
        public bool WasLearned {  get; set; }
    }
}
