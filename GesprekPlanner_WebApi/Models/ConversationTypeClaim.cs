using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GesprekPlanner_WebApi.Models
{
    public class ConversationTypeClaim
    {
        public int Id { get; set; }
        public virtual ConversationType ConversationType { get; set; }
        public virtual ApplicationUserGroup Group { get; set; }
    }
}
