using Movies.Data.Models;
using Movies.Data.Repositories;
using Movies.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Movies.Tests
{
    public class MoviesControllerTest
    {
        private readonly MovieManagementContext _context;
        private readonly MovieRepository _repository;
        private readonly MoviesController _controller;

        public MoviesControllerTest()
        {
            var options =
                new DbContextOptionsBuilder<MovieManagementContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options;

            _context = new MovieManagementContext(options);

            SeedMovies();

            _repository = new MovieRepository(_context);
            _controller = new MoviesController(_repository);
        }

        private void SeedMovies()
        {
            _context.Movies.AddRange(
                new Movie { Id = 1, Title = "Movie 1", Genre = "Drama", ReleaseYear = 2001 },
                new Movie { Id = 2, Title = "Movie 2", Genre = "Action", ReleaseYear = 2002 },
                new Movie { Id = 3, Title = "Movie 3", Genre = "Comedy", ReleaseYear = 2003 },
                new Movie { Id = 4, Title = "Movie 4", Genre = "Drama", ReleaseYear = 2004 },
                new Movie { Id = 5, Title = "Movie 5", Genre = "Action", ReleaseYear = 2005 },
                new Movie { Id = 6, Title = "Movie 6", Genre = "Comedy", ReleaseYear = 2006 },
                new Movie { Id = 7, Title = "Conan", Genre = "Adventure", ReleaseYear = 1982 },
                new Movie { Id = 8, Title = "Movie 8", Genre = "Drama", ReleaseYear = 2008 },
                new Movie { Id = 9, Title = "Movie 9", Genre = "Action", ReleaseYear = 2009 },
                new Movie { Id = 10, Title = "Movie 10", Genre = "Comedy", ReleaseYear = 2010 },
                new Movie { Id = 11, Title = "Movie 11", Genre = "Drama", ReleaseYear = 2011 },
                new Movie { Id = 12, Title = "Movie 12", Genre = "Action", ReleaseYear = 2012 },
                new Movie { Id = 13, Title = "Movie 13", Genre = "Comedy", ReleaseYear = 2013 },
                new Movie { Id = 14, Title = "Movie 14", Genre = "Drama", ReleaseYear = 2014 });

            _context.SaveChanges();
        }

        [Fact]
        public void GetAllMovies_ReturnsSuccessIfCorrectCount()
        { 
            var result = _controller.GetMovies();

            Assert.IsType<OkObjectResult>(result.Result);
            var okObject = result.Result as OkObjectResult;
            Assert.IsType<List<Movie>>(okObject.Value);
            var listMovies = ((List<Movie>)okObject.Value);
            Assert.Equal(14, listMovies.Count);
        }

        [Fact]
        public void GetAllMovies_ReturnSuccessIfWrongCount()
        {
            var result = _controller.GetMovies();

            Assert.IsType<OkObjectResult>(result.Result);
            var okObject = result.Result as OkObjectResult;
            Assert.IsType<List<Movie>>(okObject.Value);
            var listMovies = ((List<Movie>)okObject.Value);
            Assert.NotEqual(3, listMovies.Count);
        }

        [Theory]
        [InlineData(7, 1229)]
        public void GetMovieById_ReturnsOkObjectResult(int id1, int id2)
        { 
            var okResult = _controller.GetMovie(id1);
            var notFoundResult = _controller.GetMovie(id2);

            Assert.IsType<OkObjectResult>(okResult.Result);
            Assert.IsType<NotFoundResult>(notFoundResult.Result);

            var item = okResult.Result as OkObjectResult;

            Assert.IsType<Movie>(item.Value);

            var movie = item.Value as Movie;
            Assert.Equal(id1, movie.Id);
            Assert.Equal("Conan", movie.Title);
        }

        [Fact]
        public void Add_InvalidObjectPassed_ReturnsBadRequest()
        {
            var missingId = new Movie()
            {
                Genre = "Adventure",
                ReleaseYear = 2025
            };

            _controller.ModelState.AddModelError("Id", "Id is required!");
            _controller.ModelState.AddModelError("Title", "Title is required!");

            var badResponse = _controller.PostMovie(missingId);

            Assert.IsType<BadRequestResult>(badResponse.Result);
        }

        [Fact]
        public void Add_ValidObject_ReturnsCreatedResponse_and_deleted_returns_ReturnsOkObjectResult()
        {
            var newMovie = new Movie()
            {
                Title = "The Godfather",
                Genre = "Crime-Drama",
                ReleaseYear = 1972
            };

            var createdResponse = _controller.PostMovie(newMovie);

            Assert.IsType<CreatedAtActionResult>(createdResponse.Result);
            var movie = (createdResponse.Result as CreatedAtActionResult).Value as Movie;

            var deleted = _controller.DeleteMovie(movie.Id);

            Assert.IsType<OkObjectResult>(deleted);
        }

        [Theory]
        [InlineData(500000000)]
        public void Remove_NonExistingMovieById_ReturnsNotFoundResult(int id)
        { 
            var notFoundResult = _controller.DeleteMovie(id);

            Assert.IsType<NotFoundObjectResult>(notFoundResult);
        }
    }
}
