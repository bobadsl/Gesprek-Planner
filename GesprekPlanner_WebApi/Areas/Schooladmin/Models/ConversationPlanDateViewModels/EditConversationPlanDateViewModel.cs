﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GesprekPlanner_WebApi.Areas.Schooladmin.Models.ConversationPlanDateViewModels
{
    public class EditConversationPlanDateViewModel : IValidatableObject
    {
        [Required]
        public int Id { get; set; } 
        [Required]
        [Display(Name = "Groepen(Meerdere mogelijk)")]
        public List<int> SelectedGroups { get; set; }
        [Required]
        [Display(Name = "Begin Datum")]
        public string StartDate { get; set; }
        [Required]
        [Display(Name = "Eind Datum")]
        public string EndDate { get; set; }
        public IEnumerable<SelectListItem> Groups { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DateTime.ParseExact(StartDate,"dd-mm-yyyy",null) < DateTime.Now)
            {
                yield return new ValidationResult("Error: De begin datum kan niet lager zijn dan de huidige dag.");
            }
            if (DateTime.ParseExact(StartDate, "dd-mm-yyyy", null) >= DateTime.ParseExact(EndDate, "dd-mm-yyyy", null))
            {
                yield return new ValidationResult("Error: Begin datum mag niet groter of gelijk zijn aan de eind datum.");
            }
        }
    }
}
