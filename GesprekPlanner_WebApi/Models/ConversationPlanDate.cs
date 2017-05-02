using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace GesprekPlanner_WebApi.Models
{
    public class ConversationPlanDate
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Begin datum")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [Required]
        [Display(Name = "Eind datum")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        public virtual School School { get; set; }
    }
}
