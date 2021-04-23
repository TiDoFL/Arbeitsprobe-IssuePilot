using System.ComponentModel.DataAnnotations;

namespace IssuePilot.Models.ViewModels
{
    public class UserDeleteViewModel
    {
        [DataType(DataType.Text)]
        public string Id { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Benutzername")]
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
        [Display(Name = "Rolle")]
        public string Role { get; set; }
    }
}
