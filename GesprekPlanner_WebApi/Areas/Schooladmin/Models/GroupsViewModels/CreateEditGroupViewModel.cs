using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GesprekPlanner_WebApi.Areas.Schooladmin.Models.GroupsViewModels
{
    public class CreateEditGroupViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Groep")]
        [StringLength(30, ErrorMessage = "De {0} moet minimaal {2} en maximaal {1} characters lang zijn", MinimumLength = 3)]
        public string GroupName { get; set; }
    }
}
