using Grpc.Core;
using MovieSystem.Grpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
	public class MovieGrpcService : MovieService.MovieServiceBase
	{
		private readonly MyMovieService _movieService;

		public MovieGrpcService(MyMovieService movieService)
		{
			_movieService = movieService;
		}

		public override async Task<MovieResponse> GetMovie(GetMovieRequest request, ServerCallContext context)
		{
			var movie = await _movieService.GetMovieByIdAsync(request.Id);

			return new MovieResponse
			{
				Id = movie.Id,
				Title = movie.Title,
				ReleaseYear = movie.Releaseyear ?? 0,
				Rating = movie.Rating ?? 0,
				DirectorName = movie.Director?.Name ?? "Unknown",
				StudioName = movie.Studio?.Name ?? "Unknown",
				Genres = { movie.Moviegenres.Select(mg => mg.Genre.Name) },
				Actors = { movie.Movieactors.Select(ma => ma.Actor.Name) }
			};
		}

		public override async Task<GetMoviesResponse> GetMovies(GetMoviesRequest request, ServerCallContext context)
		{
			var (movies, totalCount) = await _movieService.GetMoviesAsync(
				request.PageNumber,
				request.PageSize,
				request.GenreFilter,
				request.ActorFilter,
				request.SortBy);

			var response = new GetMoviesResponse
			{
				TotalCount = totalCount
			};

			response.Movies.AddRange(movies.Select(movie => new MovieResponse
			{
				Id = movie.Id,
				Title = movie.Title,
				ReleaseYear = movie.Releaseyear ?? 0,
				Rating = movie.Rating ?? 0,
				DirectorName = movie.Director?.Name ?? "Unknown",
				StudioName = movie.Studio?.Name ?? "Unknown",
				Genres = { movie.Moviegenres.Select(mg => mg.Genre.Name) },
				Actors = { movie.Movieactors.Select(ma => ma.Actor.Name) }
			}));

			return response;
		}
	}
}
