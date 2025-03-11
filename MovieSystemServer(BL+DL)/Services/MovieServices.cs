using DAL.Models;
using DAL.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BL.Services
{
	public class MovieService
	{
		private readonly MovieRepositoryImpl _movieRepository;

		public MovieService(MovieRepositoryImpl movieRepository)
		{
			_movieRepository = movieRepository;
		}

		// Получить фильм по ID
		public async Task<Movie> GetMovieByIdAsync(int id)
		{
			return await _movieRepository.GetMovieByIdAsync(id);
		}

		// Получить все фильмы
		public async Task<List<Movie>> GetAllMoviesAsync()
		{
			return await _movieRepository.GetAllMoviesAsync();
		}

		// Получить фильмы с пагинацией, фильтрацией и сортировкой
		public async Task<(List<Movie> Movies, int TotalCount)> GetMoviesAsync(
		int pageNumber,
		int pageSize,
		string genreFilter = null,
		string actorFilter = null,
		string sortBy = null) // Новый параметр для сортировки
		{
			// Валидация параметров
			if (pageNumber < 1)
			{
				throw new ArgumentException("Page number must be greater than 0.");
			}

			if (pageSize < 1)
			{
				throw new ArgumentException("Page size must be greater than 0.");
			}

			return await _movieRepository.GetMoviesAsync(pageNumber, pageSize, genreFilter, actorFilter, sortBy);
		}

		// Добавить фильм
		public async Task AddMovieAsync(Movie movie)
		{
			// Пример бизнес-логики: проверка года выпуска
			if (movie.Releaseyear < 1900 || movie.Releaseyear > DateTime.Now.Year)
			{
				throw new ArgumentException("Invalid release year.");
			}

			await _movieRepository.AddMovieAsync(movie);
		}

		// Обновить фильм
		public async Task UpdateMovieAsync(Movie movie)
		{
			// Пример бизнес-логики: проверка рейтинга
			if (movie.Rating < 0 || movie.Rating > 10)
			{
				throw new ArgumentException("Rating must be between 0 and 10.");
			}

			await _movieRepository.UpdateMovieAsync(movie);
		}

		// Удалить фильм
		public async Task DeleteMovieAsync(int id)
		{
			await _movieRepository.DeleteMovieAsync(id);
		}

		// Поиск фильмов по названию
		public async Task<List<Movie>> SearchMoviesByTitleAsync(string title)
		{
			return await _movieRepository.SearchMoviesByTitleAsync(title);
		}

		// Получить фильмы по году выпуска
		public async Task<List<Movie>> GetMoviesByReleaseYearAsync(int year)
		{
			return await _movieRepository.GetMoviesByReleaseYearAsync(year);
		}

		// Добавить актера к фильму
		public async Task AddActorToMovieAsync(int movieId, int actorId)
		{
			await _movieRepository.AddActorToMovieAsync(movieId, actorId);
		}

		// Удалить актера из фильма
		public async Task RemoveActorFromMovieAsync(int movieId, int actorId)
		{
			await _movieRepository.RemoveActorFromMovieAsync(movieId, actorId);
		}

		// Добавить жанр к фильму
		public async Task AddGenreToMovieAsync(int movieId, int genreId)
		{
			await _movieRepository.AddGenreToMovieAsync(movieId, genreId);
		}

		// Удалить жанр из фильма
		public async Task RemoveGenreFromMovieAsync(int movieId, int genreId)
		{
			await _movieRepository.RemoveGenreFromMovieAsync(movieId, genreId);
		}
	}
}