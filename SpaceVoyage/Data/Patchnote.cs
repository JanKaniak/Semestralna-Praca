using System.ComponentModel.DataAnnotations;

namespace SpaceVoyage.Data
{
    public class Patchnote
    {
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "This field has to be filled!")]
        public string? Title { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "This field has to be filled!")]
        public string? Description { get; set; }
        public DateTime ReleaseDate { get; set; } = DateTime.Now;

        
        [Range(0,200, ErrorMessage = "Value must be between 0 and 200")]
        public int? testovaciStlpec { get; set; }
    }
}
