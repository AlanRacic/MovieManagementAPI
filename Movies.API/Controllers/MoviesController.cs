using Microsoft.AspNetCore.Mvc;
using Movies.Data.Interfaces;
using Movies.Data.Models;

namespace Movies.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieRepository _repo;

        public MoviesController(IMovieRepository repo)
        {
            _repo = repo;
        }

        // GET: api/Movies
        [HttpGet]
        public ActionResult<IEnumerable<Movie>> GetMovies()
        {
            return Ok(_repo.GetAll());
        }

        // GET: api/Movies/5
        [HttpGet("{id}")]
        public ActionResult<Movie> GetMovie(int id)
        {
            var movie = _repo.GetMovieById(id);

            if (movie == null)
            {
                return NotFound();
            }

            return Ok(movie);
        }

        // POST: api/Movies
        [HttpPost]
        public ActionResult<Movie> PostMovie(Movie movie)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var createdMovie = _repo.InsertMovie(movie);

            return CreatedAtAction(nameof(GetMovie), new { id = createdMovie.Id }, createdMovie);
        }

        // PUT: api/Movies/5
        [HttpPut("{id}")]
        public ActionResult PutMovie(int id, Movie movie)
        {
            if (id != movie.Id)
            {
                return BadRequest("Movie ID mismatch!");
            }

            var movieUpdate = _repo.UpdateMovie(movie);

            if (movieUpdate == null)
            {
                return NotFound($"Movie with ID={id} not found!");
            }

            return Ok(movieUpdate);
        }

        // DELETE: api/Movies/5
        [HttpDelete("{id}")]
        public ActionResult DeleteMovie(int id)
        {
            var deleted = _repo.DeleteMovie(id);

            if (deleted == null)
            {
                return NotFound($"Movie with ID={id} not found!");
            }

            return Ok(deleted);
        }

        // GET: api/Movies/search
        [HttpGet("search")]
        public ActionResult SearchByQueryString(
            [FromQuery] string s = "",
            [FromQuery] string orderby = "asc",
            [FromQuery] int per_page = 10,
            [FromQuery] int page = 1)
        {
            var result = _repo.QueryStringFilter(s, orderby, per_page, page);

            return Ok(result);
        }
    }
}