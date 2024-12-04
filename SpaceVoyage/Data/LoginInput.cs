using System.ComponentModel.DataAnnotations;

namespace SpaceVoyage.Data
{
    public class LoginInput
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter user name now!")]
        public string? UserName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter password now!")]
        public string? Password { get; set; }
    }
}
