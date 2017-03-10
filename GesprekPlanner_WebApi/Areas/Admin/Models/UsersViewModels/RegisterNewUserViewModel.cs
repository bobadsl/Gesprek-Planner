﻿using System;
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
        [StringLength(100, ErrorMessage = "De {0} moet minimaal {2} en maximaal {1} tekens lang zijn.", MinimumLength = 6)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email adres")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Het {0} moet minimaal {2} en maximaal {1} tekens lang zijn.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Wachtwoord")]
        public string Password { get; set; }
        [Required]
        [Display(Name = "Groep")]
        public int GroupId { get; set; }

        public IEnumerable<SelectListItem> Groups { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Herhaal wachtwoord")]
        [Compare("Password", ErrorMessage = "De wachtwoorden komen niet overeen")]
        public string ConfirmPassword { get; set; }

    }
}