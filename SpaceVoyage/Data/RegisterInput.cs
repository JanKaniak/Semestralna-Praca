using System.ComponentModel.DataAnnotations;

namespace SpaceVoyage.Data
{
    public class RegisterInput
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter user name now!")]
        public string? UserName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter password now!")]
        public string? Password { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter name now!")]
        public string? Name { get; set; }

    }
}
