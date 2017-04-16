using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GesprekPlanner_WebApi.Models
{
    public class ConversationPlanDateClaim
    {
        /*
         *This class binds the group to a conversation plan date so that the database is a lot cleaner and so its
         * a lot easier to edit and maintain things
         */
        public Guid Id { get; set; }
        public virtual ConversationPlanDate ConversationPlanDate { get; set; }
        public virtual ApplicationUserGroup Group { get; set; }
    }
}
