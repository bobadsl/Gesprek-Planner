using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace GesprekPlanner_WebApi.Models
{
    public class ConversationType
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Gesprekstype")]
        [StringLength(30, ErrorMessage = "Het {0} moet minimaal {2} en maximaal {1} tekens lang zijn", MinimumLength = 3)]
        public string ConversationName { get; set; }
        [Required]
        [Display(Name = "Gespreksduur (in minuten)")]
        public int ConversationDuration { get; set; }

        public virtual School School { get; set; }
    }
}
