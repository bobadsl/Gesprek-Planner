using System.ComponentModel.DataAnnotations;

namespace GesprekPlanner_WebApi.Areas.Schooladmin.Models.ConversationPlanDateViewModels
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
