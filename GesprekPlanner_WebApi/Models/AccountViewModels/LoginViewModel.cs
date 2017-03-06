using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GesprekPlanner_WebApi.Models.AccountViewModels
{
    public class LoginViewModel
    {
        [Required]
        [StringLength(40, ErrorMessage = "De gebruikersnaam mag maximaal 40 tekens lang zijn")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Gegevens onthouden?")]
        public bool RememberMe { get; set; }
    }
}
