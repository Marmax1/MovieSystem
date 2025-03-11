using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories
{
	public class MovieRepositoryImpl
	{
		private readonly MovieDatabaseContext _context;

		public MovieRepositoryImpl(MovieDatabaseContext context)
		{
			_context = context;
		}

		// Получить фильм по ID
		public async Task<Movie> GetMovieByIdAsync(int id)
		{
			return await _context.Movies
				.Include(m => m.Director) // Включаем режиссера
				.Include(m => m.Studio) // Включаем студию
				.Include(m => m.Movieactors) // Включаем актеров
					.ThenInclude(ma => ma.Actor)
				.Include(m => m.Moviegenres) // Включаем жанры
					.ThenInclude(mg => mg.Genre)
				.FirstOrDefaultAsync(m => m.Id == id);
		}

		// Получить все фильмы
		public async Task<List<Movie>> GetAllMoviesAsync()
		{
			return await _context.Movies
				.Include(m => m.Director)
				.Include(m => m.Studio)
				.Include(m => m.Movieactors)
					.ThenInclude(ma => ma.Actor)
				.Include(m => m.Moviegenres)
					.ThenInclude(mg => mg.Genre)
				.ToListAsync();
		}

		// Получить фильмы с пагинацией, фильтрацией и сортировкой
		public async Task<(List<Movie> Movies, int TotalCount)> GetMoviesAsync(
			int pageNumber,
			int pageSize,
			string genreFilter = null,
			string actorFilter = null,
			string sortBy = null) // Новый параметр для сортировки
		{
			var query = _context.Movies
				.Include(m => m.Director)
				.Include(m => m.Studio)
				.Include(m => m.Movieactors)
					.ThenInclude(ma => ma.Actor)
				.Include(m => m.Moviegenres)
					.ThenInclude(mg => mg.Genre)
				.AsQueryable();

			// Фильтрация по жанру
			if (!string.IsNullOrEmpty(genreFilter))
			{
				query = query.Where(m => m.Moviegenres.Any(mg => mg.Genre.Name.Contains(genreFilter)));
			}

			// Фильтрация по актеру
			if (!string.IsNullOrEmpty(actorFilter))
			{
				query = query.Where(m => m.Movieactors.Any(ma => ma.Actor.Name.Contains(actorFilter)));
			}

			// Сортировка
			switch (sortBy?.ToLower())
			{
				case "title":
					query = query.OrderBy(m => m.Title);
					break;
				case "title_desc":
					query = query.OrderByDescending(m => m.Title);
					break;
				case "year":
					query = query.OrderBy(m => m.Releaseyear);
					break;
				case "year_desc":
					query = query.OrderByDescending(m => m.Releaseyear);
					break;
				case "rating":
					query = query.OrderBy(m => m.Rating);
					break;
				case "rating_desc":
					query = query.OrderByDescending(m => m.Rating);
					break;
				default:
					query = query.OrderBy(m => m.Id); // Сортировка по умолчанию
					break;
			}

			// Подсчет общего количества фильмов (для пагинации)
			int totalCount = await query.CountAsync();

			// Пагинация
			var movies = await query
				.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();

			return (movies, totalCount);
		}

		// Добавить новый фильм
		public async Task AddMovieAsync(Movie movie)
		{
			await _context.Movies.AddAsync(movie);
			await _context.SaveChangesAsync();
		}

		// Обновить фильм
		public async Task UpdateMovieAsync(Movie movie)
		{
			_context.Movies.Update(movie);
			await _context.SaveChangesAsync();
		}

		// Удалить фильм по ID
		public async Task DeleteMovieAsync(int id)
		{
			var movie = await _context.Movies.FindAsync(id);
			if (movie != null)
			{
				_context.Movies.Remove(movie);
				await _context.SaveChangesAsync();
			}
		}

		// Поиск фильмов по названию
		public async Task<List<Movie>> SearchMoviesByTitleAsync(string title)
		{
			return await _context.Movies
				.Where(m => m.Title.Contains(title))
				.Include(m => m.Director)
				.Include(m => m.Studio)
				.Include(m => m.Movieactors)
					.ThenInclude(ma => ma.Actor)
				.Include(m => m.Moviegenres)
					.ThenInclude(mg => mg.Genre)
				.ToListAsync();
		}

		// Получить фильмы по году выпуска
		public async Task<List<Movie>> GetMoviesByReleaseYearAsync(int year)
		{
			return await _context.Movies
				.Where(m => m.Releaseyear == year)
				.Include(m => m.Director)
				.Include(m => m.Studio)
				.Include(m => m.Movieactors)
					.ThenInclude(ma => ma.Actor)
				.Include(m => m.Moviegenres)
					.ThenInclude(mg => mg.Genre)
				.ToListAsync();
		}

		// Добавить актера к фильму
		public async Task AddActorToMovieAsync(int movieId, int actorId)
		{
			var movieActor = new Movieactor
			{
				Movieid = movieId,
				Actorid = actorId
			};

			await _context.Movieactors.AddAsync(movieActor);
			await _context.SaveChangesAsync();
		}

		// Удалить актера из фильма
		public async Task RemoveActorFromMovieAsync(int movieId, int actorId)
		{
			var movieActor = await _context.Movieactors
				.FirstOrDefaultAsync(ma => ma.Movieid == movieId && ma.Actorid == actorId);

			if (movieActor != null)
			{
				_context.Movieactors.Remove(movieActor);
				await _context.SaveChangesAsync();
			}
		}

		// Добавить жанр к фильму
		public async Task AddGenreToMovieAsync(int movieId, int genreId)
		{
			var movieGenre = new Moviegenre
			{
				Movieid = movieId,
				Genreid = genreId
			};

			await _context.Moviegenres.AddAsync(movieGenre);
			await _context.SaveChangesAsync();
		}

		// Удалить жанр из фильма
		public async Task RemoveGenreFromMovieAsync(int movieId, int genreId)
		{
			var movieGenre = await _context.Moviegenres
				.FirstOrDefaultAsync(mg => mg.Movieid == movieId && mg.Genreid == genreId);

			if (movieGenre != null)
			{
				_context.Moviegenres.Remove(movieGenre);
				await _context.SaveChangesAsync();
			}
		}
	}
}