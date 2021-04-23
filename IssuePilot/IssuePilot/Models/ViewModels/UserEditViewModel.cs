using System.ComponentModel.DataAnnotations;

namespace IssuePilot.Models.ViewModels
{
    public class UserEditViewModel
    {
        [DataType(DataType.Text)]
        public string Id { get; set; }


        [DataType(DataType.Text)]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Vorname")]
        public string Firstname { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Nachname")]
        public string Surname { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Passwort")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Passwort bestätigen")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Role { get; set; }

        [Display(Name = "Aktuelle Rolle")]
        public string CurrentRole { get; set; }
    }
}
