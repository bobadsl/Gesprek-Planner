using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GesprekPlanner_WebApi.Areas.Schooladmin.Models.ConversationTypeViewModels
{
    public class CreateConversationTypeViewModel
    {
        [Required]
        [StringLength(30, ErrorMessage = "Het {0} moet minimaal {2} en maximaal {1} tekens lang zijn", MinimumLength = 3)]
        [Display(Name="Gesprekstype")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Gespreksduur (in minuten)")]
        public int Duration { get; set; }
        [Required]
        [Display(Name="Groep(en)")]
        public List<int> SelectedGroups { get; set; }
        public IEnumerable<SelectListItem> Groups { get; set; }
    }
}
