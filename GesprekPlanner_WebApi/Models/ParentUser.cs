using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GesprekPlanner_WebApi.Models
{
    public class ParentUser
    {
        public Guid Id { get; set; }
        [Required]
        [Display(Name="Email adres (Ouder 1)")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Display(Name="Email adres (Ouder 2)")]
        [DataType(DataType.EmailAddress)]
        public string AltEmail { get; set; }
        [Required]
        [Display(Name="Naam van kind")]
        public string ChildName { get; set; } // Is unique
        public virtual School School { get; set; }
    }
}
