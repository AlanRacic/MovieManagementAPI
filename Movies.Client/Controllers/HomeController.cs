using System.Diagnostics;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Movies.Client.Models;

namespace Movies.Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> GetMovies()
        {
            var client = _httpClientFactory.CreateClient("MoviesApi");

            var movies = await client.GetFromJsonAsync<List<Movie>>("api/Movies");

            return View(movies ?? new List<Movie>());
        }

        public IActionResult CreateMovie()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateMovie(Movie movie)
        {
            ModelState.Remove("Id");

            if (!ModelState.IsValid)
            {
                return View(movie);
            }

            var client = _httpClientFactory.CreateClient("MoviesApi");

            var response = await client.PostAsJsonAsync("api/Movies", movie);

            response.EnsureSuccessStatusCode();

            return RedirectToAction(nameof(GetMovies));
        }

        public async Task<IActionResult> GetMovie(int id)
        {
            var client = _httpClientFactory.CreateClient("MoviesApi");

            var movie = await client.GetFromJsonAsync<Movie>($"api/Movies/{id}");

            if (movie is null)
            {
                return NotFound();
            }

            return View(movie);
        }
    }
}
