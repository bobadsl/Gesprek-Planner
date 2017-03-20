using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GesprekPlanner_WebApi.Models;

namespace GesprekPlanner_WebApi.Areas.Teacher.Models.ConversationViewModels
{
    public class ConversationIndexViewModel
    {
        public List<Conversation> Conversations { get; set; }
        public List<ConversationPlanDate> ConversationPlanDates { get; set; }
    }
}
