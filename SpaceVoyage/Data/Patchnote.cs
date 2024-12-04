namespace SpaceVoyage.Data
{
    public class Patchnote
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime ReleaseDate { get; set; } = DateTime.Now;
        public byte[]? Image { get; set; }
    }
}
