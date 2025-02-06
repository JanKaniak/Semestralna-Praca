using System.ComponentModel.DataAnnotations;

namespace SpaceVoyage.Data
{
    public class Review
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        [Range(0,5, ErrorMessage = "This field has to be filled!")]
        public int Rating { get; set; } = 0;

        [Required(AllowEmptyStrings = false, ErrorMessage = "This field has to be filled!")]
        public string? Header { get; set; }
        public DateTime ReleaseDate { get; set; } = DateTime.Now;
        [Required(AllowEmptyStrings = false, ErrorMessage = "This field has to be filled!")]
        public string? Text { get; set; }
    }
}
