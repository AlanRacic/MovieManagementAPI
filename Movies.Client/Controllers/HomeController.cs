using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Movies.Client.Models;
using System.Text.Json;

namespace Movies.Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7008/api/Movies");
            var content = new StringContent("\"test\"", null, "application/json");
            request.Content = content;
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            string result = await response.Content.ReadAsStringAsync();
            JsonSerializerOptions jo = new JsonSerializerOptions() 
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            List<Movie> movies = JsonSerializer.Deserialize<List<Movie>>(result, jo);
            return View(movies);
        }

        public async Task<IActionResult> CreateMovie()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateMovie(Movie movie)
        {
            ModelState.Remove("Id");
            if (ModelState.IsValid)
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7008/api/Movies");
                var content = new StringContent(JsonSerializer.Serialize(movie), null, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                //Console.WriteLine(await response.Content.ReadAsStringAsync());
                return RedirectToAction(nameof(GetMovies));
            }
            return View(movie);
        }

        public async Task<IActionResult> GetMovie(int id)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7008/api/Movies/" + id.ToString());
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
            }
            var result = await response.Content.ReadAsStringAsync();
            JsonSerializerOptions jo = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            Movie movie = JsonSerializer.Deserialize<Movie>(result, jo);
            return View(movie);
        }
    }
}
