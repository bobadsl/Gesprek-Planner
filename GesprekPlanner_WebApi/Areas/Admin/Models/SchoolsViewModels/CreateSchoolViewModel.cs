using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace GesprekPlanner_WebApi.Areas.Admin.Models.SchoolsViewModels
{
    public class CreateSchoolViewModel
    {
        [Required]
        public string SchoolName { get; set; }
        [Required]
        public string SchoolUrl { get; set; }
        [Required]
        [StringLength(20, ErrorMessage = "Het telefoonnummer mag niet meer dan 20 tekens bevatten")]
        [RegularExpression(@"^0[1-68]([ .-]?[0-9]{2}){4}$", ErrorMessage = "Ongeldige tekens gevonden")]
        public string SchoolTelephone { get; set; }
        [Required]
        [RegularExpression(@"([\w\.]+)@([\w\.]+)\.(\w+)", ErrorMessage = "Dit is geen geldig email adres")]
        public string SchoolEmail { get; set; }
        [Required]
        public string SchoolStreet { get; set; }
        [Required]
        public string SchoolPostCode { get; set; }
        public IFormFile SchoolLogo { get; set; }
    }
}
