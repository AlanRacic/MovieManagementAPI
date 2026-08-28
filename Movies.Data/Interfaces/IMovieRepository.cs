using Movies.Data.Models;

namespace Movies.Data.Interfaces
{
    public interface IMovieRepository
    {
        IEnumerable<Movie> GetAll();
        Movie? GetMovieById(int id);
        Movie InsertMovie(Movie movie);
        Movie? UpdateMovie(Movie movie);
        Movie? DeleteMovie(int id);

        IEnumerable<Movie> QueryStringFilter(string? search, string orderBy, int perPage, int page);
    }
}
