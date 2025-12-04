using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            _context = new MovieManagementContext();
            _repository = new MovieRepository(_context);
            _controller = new MoviesController(_repository);
        }

        [Fact]
        public void GetAllMovies_ReturnsSuccessIfCorrectCount()
        { 
            var result = _controller.GetMovies();

            Assert.IsType<OkObjectResult>(result.Result.Result);
            var okobject = result.Result.Result as OkObjectResult;
            Assert.IsType<List<Movie>>(okobject.Value);
            var listMovies = ((List<Movie>)okobject.Value);
            Assert.Equal(14, listMovies.Count);
        }

        [Fact]
        public void GetAllMovies_ReturnSuccessIfWrongCount()
        {
            var result = _controller.GetMovies();

            Assert.IsType<OkObjectResult>(result.Result.Result);
            var okobject = result.Result.Result as OkObjectResult;
            Assert.IsType<List<Movie>>(okobject.Value);
            var listMovies = ((List<Movie>)okobject.Value);
            Assert.NotEqual(3, listMovies.Count);
        }

        [Theory]
        [InlineData(7, 1229)]
        public void GetMovieById_ReturnsOkObjectResult(int id1, int id2)
        { 
            var okResult = _controller.GetMovie(id1);
            var notFoundResult = _controller.GetMovie(id2);

            Assert.IsType<OkObjectResult>(okResult.Result.Result);
            Assert.IsType<NotFoundResult>(notFoundResult.Result.Result);

            var item = okResult.Result.Result as OkObjectResult;

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

            Assert.IsType<BadRequestResult>(badResponse.Result.Result);
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

            Assert.IsType<CreatedAtActionResult>(createdResponse.Result.Result);
            var movie = (createdResponse.Result.Result as CreatedAtActionResult).Value as Movie;

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
