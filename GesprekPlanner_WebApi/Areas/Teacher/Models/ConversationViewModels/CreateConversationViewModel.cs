using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GesprekPlanner_WebApi.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GesprekPlanner_WebApi.Areas.Teacher.Models.ConversationViewModels
{
    public class CreateConversationViewModel
    {
        [Required]
        [Display(Name="Gespreks type")]
        public int ConversationType { get; set; }
        [Required]
        [Display(Name = "Datums")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public List<DateTime> PlanOnDate { get; set; }
        public List<SelectListItem> PlannableDates { get; set; }
        public List<SelectListItem> ConversationTypes { get; set; }
    }
}
