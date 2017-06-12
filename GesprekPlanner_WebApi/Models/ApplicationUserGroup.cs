using System;
using System.ComponentModel.DataAnnotations;

namespace GesprekPlanner_WebApi.Models
{
    public class ApplicationUserGroup
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Groep")]
        [StringLength(30, ErrorMessage = "De {0} moet minimaal {2} en maximaal {1} characters lang zijn", MinimumLength = 3)]
        public string GroupName { get; set; }
        [Display(Name="Mailgroep")]
        public string EmailGroup { get; set; }

        public virtual School School { get; set; }
    }
}
