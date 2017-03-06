using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GesprekPlanner_WebApi.Models
{
    public class MinimalUser
    {
        [Display(Name = "ID")]
        public string Id { get; set; }
        [Display(Name="Gebruikersnaam")]
        public string UserName { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name="Groeps id")]
        public int GroupId { get; set; }
    }
}
