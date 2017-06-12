using System.ComponentModel.DataAnnotations;

namespace GesprekPlanner_WebApi.Areas.Schooladmin.Models.ConversationTypeViewModels
{
    public class ListConversationTypeViewModel
    {
        public int Id { get; set; }
        [Display(Name="Gespreksnaam")]
        public string ConversationName { get; set; }
        [Display(Name = "Gespreksduur (in minuten)")]
        public int Duration { get; set; }
        [Display(Name="Groep")]
        public string Group { get; set; }
    }
}
