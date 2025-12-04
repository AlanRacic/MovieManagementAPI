using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
        {
            try
            {
                return Ok(_repo.GetAll());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error: " + ex.Message);
            }
        }

        // GET: api/Movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovie(int id)
        {
            try
            {
                var movie = _repo.GetmovieById(id);

                if (movie == null) return NotFound();
                
                return Ok(movie);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error: " + ex.Message);
            }
        }

        // POST: api/Movies
        [HttpPost]
        public async Task<ActionResult<Movie>> PostMovie(Movie movie)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var created_movie = _repo.InsertMovie(movie);

                return CreatedAtAction(nameof(GetMovie), new { id = created_movie.Id }, created_movie);
            }
            catch (Exception ex) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error: " + ex.Message);
            }
        }

        // PUT: api/Movies/5
        [HttpPut("{id}")]
        public ActionResult PutMovie(int id, Movie movie)
        {
            try
            {
                if (id != movie.Id)
                {
                    return BadRequest("Movie ID mismatch!");
                }
                var movieUpdate = _repo.UpdateMovie(movie);
                if (movieUpdate == null) return NotFound($"Movie with ID={id} not found!");
                return Ok(movieUpdate);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error: " + ex.Message);
            }
        }

        // DELETE: api/Movies/5
        [HttpDelete("{id}")]
        public ActionResult DeleteMovie(int id)
        {
            try
            {
                var deleted = _repo.DeleteMovie(id);
                if (deleted == null) return NotFound($"Movie with ID={id} not found!");
                return Ok(deleted);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error: " + ex.Message);
            }
        }

        // GET: api/Movies/search
        [HttpGet("search")]
        public ActionResult SearchByQueryString([FromQuery] string s = "", [FromQuery] string orderby = "asc", [FromQuery] int per_page = 10, [FromQuery] int page = 1)
        {
            try
            {
                var result = _repo.QueryStringfilter(s, orderby, per_page, page);
                if (result == null) return NotFound();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error: " + ex.Message);
            }
        }
    }
}
