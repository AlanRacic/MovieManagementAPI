using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Movies.Data.Models;
using Movies.API.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Movies.Tests
{
    public class MoviesControllerTestWithDummyDb
    {
        [Fact]
        public void GetAllMovies_ReturnsSuccessIfCorrectCount()
        {
            var repo = new TestRepo();
            var controller = new MoviesController(repo);

            var result = controller.GetMovies();

            Assert.IsType<OkObjectResult>(result.Result.Result);
            Assert.IsType<List<Movie>>((result.Result.Result as OkObjectResult).Value);
            var list = (result.Result.Result as OkObjectResult).Value as List<Movie>;
            Assert.Equal(7, list.Count);
        }

        [Fact]
        public void GetAllMovies_ReturnSuccessIfWrongCount()
        {
            var repo = new TestRepo();
            var controller = new MoviesController(repo);

            var result = controller.GetMovies();

            Assert.IsType<OkObjectResult>(result.Result.Result);
            var okobject = result.Result.Result as OkObjectResult;
            Assert.IsType<List<Movie>>(okobject.Value);
            var listMovies = ((List<Movie>)okobject.Value);
            Assert.NotEqual(3, listMovies.Count);
        }

        [Fact]
        public void Add_ValidObject_ReturnsCreatedResponse()
        {
            var repo = new TestRepo();
            var controller = new MoviesController(repo);
            var newMovie = new Movie()
            {
                Id = 8,
                Title = "The Godfather",
                Genre = "Crime-Drama",
                ReleaseYear = 1972
            };

            var createdResponse = controller.PostMovie(newMovie);

            Assert.IsType<CreatedAtActionResult>(createdResponse.Result.Result);
            Assert.Equal(8, repo.GetAll().ToList().Count);
        }

        [Theory]
        [InlineData(2, "Predator")]
        [InlineData(7, "Superman")]
        public void GetMovieById_ReturnsOkObjectResult(int id, string title)
        {
            var repo = new TestRepo();
            var controller = new MoviesController(repo);

            var okResult = controller.GetMovie(id);

            Assert.IsType<OkObjectResult>(okResult.Result.Result);
            var movie = (okResult.Result.Result as OkObjectResult).Value as Movie;
            Assert.Equal(id, movie.Id);
            Assert.Equal(title, movie.Title);
        }

        [Fact]
        public void GetMovieById_ReturnsNotFoundResult() 
        {
            var repo = new TestRepo();
            var controller = new MoviesController(repo);

            var notfound = controller.GetMovie(12);

            Assert.IsType<NotFoundResult>(notfound.Result.Result);
        }

        [Fact]
        public void Add_InvalidObjectPassed_ReturnsBadRequest()
        {
            var repo = new TestRepo();
            var controller = new MoviesController(repo);
            var newMovie = new Movie()
            {
                Genre = "Crime-Drama",
                ReleaseYear = 1972
            };

            controller.ModelState.AddModelError("Id", "Id is required");
            controller.ModelState.AddModelError("Title", "Title is required");

            var badresponse = controller.PostMovie(newMovie);

            Assert.IsType<BadRequestResult>(badresponse.Result.Result);
        }

        [Theory]
        [InlineData(2, "Predator")]
        [InlineData(7, "Superman")]
        public void Remove_ReturnsOkObjectResult(int id, string title)
        {
            var repo = new TestRepo();
            var controller = new MoviesController(repo);

            var okResult = controller.DeleteMovie(id);

            Assert.IsType<OkObjectResult>(okResult);
            var movie = (okResult as ObjectResult).Value as Movie;
            Assert.Equal(id, movie.Id);
            Assert.Equal(title, movie.Title);

            Assert.Equal(6, repo.GetAll().ToList().Count);
        }

        [Theory]
        [InlineData(100)]
        public void Remove_NonExistingObject_ReturnsNotFound(int id)
        {
            var repo = new TestRepo();
            var controller = new MoviesController(repo);

            var notfound = controller.DeleteMovie(id);

            Assert.IsType<NotFoundObjectResult>(notfound);

            Assert.Equal(7, repo.GetAll().ToList().Count);
        }
    }
}
