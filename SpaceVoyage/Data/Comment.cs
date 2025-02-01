using Microsoft.Extensions.Configuration.UserSecrets;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpaceVoyage.Data
{
    public class Comment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "This field has to be filled!")]
        public string? Text { get; set; }
    }
}
