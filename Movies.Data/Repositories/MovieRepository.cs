using Microsoft.EntityFrameworkCore;
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
            return _context.Movies.AsNoTracking().ToList();
        }

        public Movie? GetMovieById(int id)
        {
            return _context.Movies.AsNoTracking().FirstOrDefault(m => m.Id == id);
        }

        public Movie InsertMovie(Movie movie)
        {
            var result = _context.Movies.Add(movie);
            _context.SaveChanges();
            return result.Entity;
        }

        public Movie? UpdateMovie(Movie movie)
        {
            var dbMovie = _context.Movies.FirstOrDefault(m => m.Id == movie.Id);

            if (dbMovie is null)
            {
                return null;
            }

            dbMovie.Title = movie.Title;
            dbMovie.Genre = movie.Genre;
            dbMovie.ReleaseYear = movie.ReleaseYear;

            _context.SaveChanges();

            return dbMovie;
        }

        public Movie? DeleteMovie(int id)
        {
            var movie = _context.Movies.FirstOrDefault(m => m.Id == id);

            if (movie is null)
            {
                return null;
            }

            _context.Movies.Remove(movie);
            _context.SaveChanges();

            return movie;
        }

        public IEnumerable<Movie> QueryStringFilter(string? search, string orderBy, int perPage, int page)
        {
            var query = _context.Movies.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(movie => movie.Title.Contains(search));
            }

            query = orderBy.ToLowerInvariant() switch
            {
                "desc" => query.OrderByDescending(movie => movie.Title),
                _ => query.OrderBy(movie => movie.Title)
            };

            if (perPage > 0)
            {
                page = Math.Max(page, 1);

                query = query
                    .Skip(perPage * (page - 1))
                    .Take(perPage);
            }

            return query.ToList();
        }
    }
}
