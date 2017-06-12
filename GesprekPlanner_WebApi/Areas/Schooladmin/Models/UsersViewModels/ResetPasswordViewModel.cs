using System.ComponentModel.DataAnnotations;

namespace GesprekPlanner_WebApi.Areas.Schooladmin.Models.UsersViewModels
{
    public class ResetPasswordViewModel
    {
        [Required]
        public string Id { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Het {0} moet minimaal {2} en maximaal {1} tekens lang zijn.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nieuw wachtwoord")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Herhaal wachtwoord")]
        [Compare("Password", ErrorMessage = "De wachtwoorden komen niet overeen")]
        public string ConfirmPassword { get; set; }
    }
}
