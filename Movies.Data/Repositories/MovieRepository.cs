using Movies.Data.Interfaces;
using Movies.Data.Models;

namespace Movies.Data.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly MovieManagementContext _context;
        public MovieRepository(MovieManagementContext context)
        {
            _context = context;
        }

        public IEnumerable<Movie> GetAll()
        {
            return _context.Movies.ToList();
        }

        public Movie GetmovieById(int id)
        {
            return _context.Movies.FirstOrDefault(m => m.Id == id);
        }

        public Movie InsertMovie(Movie movie)
        {
            var result = _context.Movies.Add(movie);
            _context.SaveChanges();
            return result.Entity;
        }

        public Movie UpdateMovie(Movie movie)
        {
            var dbmovie = _context.Movies.FirstOrDefault(m => m.Id == movie.Id);
            if (dbmovie != null)
            {
                dbmovie.Title = movie.Title;
                dbmovie.Genre = movie.Genre;
                dbmovie.ReleaseYear = movie.ReleaseYear;

                _context.SaveChanges();
                return dbmovie;
            }

            return null;
        }

        public Movie DeleteMovie(int id)
        {
            var movie = _context.Movies.FirstOrDefault(m => m.Id == id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
                _context.SaveChanges();
                return movie;
            }

            return null;
        }

        public IEnumerable<Movie> QueryStringfilter(string s, string orderby, int per_page, int page)
        {
            var movies = _context.Movies.ToList();

            if (!String.IsNullOrEmpty(s))
            {
                movies = movies.Where(m => m.Title.Contains(s)).ToList();
            }

            switch (orderby)
            {
                case "asc":
                    movies = movies.OrderBy(m => m.Title).ToList();
                    break;
                case "desc":
                    movies = movies.OrderByDescending(m => m.Title).ToList();
                    break;
            }

            if (per_page > 0)
            {
                if (page < 1) page = 1;
                movies = movies.Skip(per_page * (page - 1)).Take(per_page).ToList();
            }

            return movies;
        }
    }
}
