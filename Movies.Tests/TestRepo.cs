using Movies.Data.Interfaces;
using Movies.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Tests
{
    public class TestRepo : IMovieRepository
    {
        private List<Movie> _movies;
        public TestRepo()
        {
            _movies = new List<Movie>();
            _movies.Add(new Movie { Id = 1, Title = "300", Genre = "Action", ReleaseYear = 2005 });
            _movies.Add(new Movie { Id = 2, Title = "Predator", Genre = "Action", ReleaseYear = 1988 });
            _movies.Add(new Movie { Id = 3, Title = "Alien", Genre = "Action", ReleaseYear = 1978 });
            _movies.Add(new Movie { Id = 4, Title = "Star Trek", Genre = "Action", ReleaseYear = 2008 });
            _movies.Add(new Movie { Id = 5, Title = "Independence day", Genre = "Action", ReleaseYear = 1996 });
            _movies.Add(new Movie { Id = 6, Title = "5th element", Genre = "Action", ReleaseYear = 1995 });
            _movies.Add(new Movie { Id = 7, Title = "Superman", Genre = "Action", ReleaseYear = 2012 });
        }

        public Movie DeleteMovie(int id)
        {
            var movie = _movies.Where(m => m.Id == id).FirstOrDefault();
            if (movie != null)
            {
                _movies.Remove(movie);
            }
            return movie;
        }

        public IEnumerable<Movie> GetAll()
        {
            return _movies;
        }

        public Movie GetmovieById(int id)
        {
            return _movies.Where(m => m.Id == id).FirstOrDefault();
        }

        public Movie InsertMovie(Movie movie)
        {
            _movies.Add(movie);
            return movie;
        }

        public IEnumerable<Movie> QueryStringfilter(string s, string orderby, int per_page, int page)
        {
            throw new NotImplementedException();
        }

        public Movie UpdateMovie(Movie movie)
        {
            var updateMovie = _movies.Where(m => m.Id == movie.Id).FirstOrDefault();
            if (updateMovie != null)
            {
                updateMovie.Title = movie.Title;
                updateMovie.Genre = movie.Genre;
                updateMovie.ReleaseYear = movie.ReleaseYear;
            }
            return updateMovie;
        }
    }
}
