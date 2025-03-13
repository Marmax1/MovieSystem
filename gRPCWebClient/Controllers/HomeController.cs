using gRPCWebClient.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using gRPCWebClient.Models;
using MovieSystem.Grpc;

namespace gRPCWebClient.Controllers
{
	public class HomeController : Controller
	{
		private readonly MovieService.MovieServiceClient _movieService;
		private readonly ILogger<HomeController> _logger;

		public HomeController(MovieService.MovieServiceClient movieService, ILogger<HomeController> logger)
		{
			_movieService = movieService;
			_logger = logger;
		}

		public async Task<IActionResult> Index(MoviesFilterModel filter)
		{
			var request = new GetMoviesRequest
			{
				PageNumber = filter.PageNumber,
				PageSize = filter.PageSize,
				GenreFilter = filter.GenreFilter ?? "",
				ActorFilter = filter.ActorFilter ?? "",
				SortBy = filter.SortBy ?? ""
			};

			var response = await _movieService.GetMoviesAsync(request);

			var movies = response.Movies.Select(m => new MovieViewModel
			{
				Id = m.Id,
				Title = m.Title,
				ReleaseYear = m.ReleaseYear,
				Rating = m.Rating,
				DirectorName = m.DirectorName,
				StudioName = m.StudioName,
				Genres = m.Genres.ToList(),
				Actors = m.Actors.ToList()
			}).ToList();

			ViewBag.TotalCount = response.TotalCount;
			ViewBag.Filter = filter;

			return View(movies);
		}

		public async Task<IActionResult> Details(int id)
		{
			var request = new GetMovieRequest { Id = id };
			var response = await _movieService.GetMovieAsync(request);

			var movie = new MovieViewModel
			{
				Id = response.Id,
				Title = response.Title,
				ReleaseYear = response.ReleaseYear,
				Rating = response.Rating,
				DirectorName = response.DirectorName,
				StudioName = response.StudioName,
				Genres = response.Genres.ToList(),
				Actors = response.Actors.ToList()
			};

			return View(movie);
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
	}
}
