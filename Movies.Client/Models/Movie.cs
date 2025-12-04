namespace Movies.Client.Models
{
    public class Movie
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string? Genre { get; set; }

        public int? ReleaseYear { get; set; }
    }
}
