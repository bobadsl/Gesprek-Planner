using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GesprekPlanner_WebApi.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GesprekPlanner_WebApi.Areas.Admin.Models.GroupsViewModels
{
    public class CreateEditGroupViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Groep")]
        [StringLength(30, ErrorMessage = "De {0} moet minimaal {2} en maximaal {1} characters lang zijn", MinimumLength = 3)]
        public string GroupName { get; set; }
        [Display(Name = "Mailgroep")]
        public string EmailGroup { get; set; }
        [Display(Name ="Selecteer een School")]
        [Required]
        public string SelectedSchool { get; set; }
        public IEnumerable<SelectListItem> Schools { get; set; }
    }
}
