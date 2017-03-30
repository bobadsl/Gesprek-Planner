using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GesprekPlanner_WebApi.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GesprekPlanner_WebApi.Areas.Admin.Models.UsersViewModels
{
    public class RegisterNewUserViewModel
    {
        [Required]
        [Display(Name = "Gebruikersnaam")]
        [RegularExpression("([<>()/])", ErrorMessage = "<> () / zijn illegale tekens")]
        [StringLength(100, ErrorMessage = "De {0} moet minimaal {2} en maximaal {1} tekens lang zijn.", MinimumLength = 6)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [RegularExpression("([<>()/])", ErrorMessage = "<> () / zijn illegale tekens")]
        [Display(Name = "Email adres")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Het {0} moet minimaal {2} en maximaal {1} tekens lang zijn.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Wachtwoord")]
        public string Password { get; set; }
        [Required]
        [Display(Name = "Groep")]
        [RegularExpression("([<>()/])", ErrorMessage = "<> () / zijn illegale tekens")]
        public string Group { get; set; }

        public string Groups { get; set; } // This is a Json string


        [Required]
        [Display(Name="Functie")]
        public string RoleName { get; set; }
        public IEnumerable<SelectListItem> Roles { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Herhaal wachtwoord")]
        [Compare("Password", ErrorMessage = "De wachtwoorden komen niet overeen")]
        public string ConfirmPassword { get; set; }

    }
}
