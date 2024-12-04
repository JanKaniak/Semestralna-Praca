using System.ComponentModel.DataAnnotations;

namespace SpaceVoyage.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter user name now!")]
        public string? UserName { get; set; }
        
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter password now!")]
        public string? Password { get; set; }
    }
}
