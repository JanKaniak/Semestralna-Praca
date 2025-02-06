namespace SpaceVoyage.Data
{
    public class Review
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int Rating { get; set; }
        public string? Header { get; set; }
        public DateTime ReleaseDate { get; set; } = DateTime.Now;
        public string? Text { get; set; }
    }
}
