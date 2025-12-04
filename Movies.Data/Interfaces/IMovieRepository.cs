using Movies.Data.Models;

namespace Movies.Data.Interfaces
{
    public interface IMovieRepository
    {
        IEnumerable<Movie> GetAll();
        Movie GetmovieById(int id);
        Movie InsertMovie(Movie movie);
        Movie UpdateMovie(Movie movie);
        Movie DeleteMovie(int id);

        IEnumerable<Movie> QueryStringfilter(string s, string orderby, int per_page, int page);
    }
}
