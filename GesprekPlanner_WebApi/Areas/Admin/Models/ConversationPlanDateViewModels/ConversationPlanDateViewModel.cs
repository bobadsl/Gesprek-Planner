using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GesprekPlanner_WebApi.Areas.Admin.Models.ConversationPlanDateViewModels
{
    public class ConversationPlanDateViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Begin datum")]
        public string StartDate { get; set; }
        [Display(Name = "Eind datum")]
        public string EndDate { get; set; }
        [Display(Name = "Groep")]
        public string Group { get; set; }
    }
}
