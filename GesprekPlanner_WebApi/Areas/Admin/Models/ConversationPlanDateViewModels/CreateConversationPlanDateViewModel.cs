using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GesprekPlanner_WebApi.Areas.Admin.Models.ConversationPlanDateViewModels
{
    public class CreateConversationPlanDateViewModel : IValidatableObject
    {
        [Required]
        [Display(Name = "Begin datum")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }
        [Required]
        [Display(Name = "Eind datum")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }
        [Required]
        [Display(Name ="Groepen(Meerdere mogelijk)")]
        public List<int> SelectedGroups { get; set; }

        public IEnumerable<SelectListItem> Groups { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (StartDate >= EndDate)
            {
                yield return new ValidationResult("Error: Begin datum mag niet groter of gelijk zijn aan de eind datum.");
            }
        }
    }
}
