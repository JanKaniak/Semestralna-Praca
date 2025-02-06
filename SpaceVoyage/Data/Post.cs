using System.ComponentModel.DataAnnotations;

namespace SpaceVoyage.Data
{
    public class Post
    {
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "This field has to be filled!")]
        public string? Title { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "This field has to be filled!")]
        public string? Description { get; set; }
        public DateTime ReleaseDate { get; set; } = DateTime.Now;

        public string? FilePath { get; set; }

        public string? Type { get; set; }

        public int UserId { get; set; }
    }
}
