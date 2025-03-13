public class MoviesFilterModel
{
	public int PageNumber { get; set; } = 1;
	public int PageSize { get; set; } = 10;
	public string GenreFilter { get; set; } = ""; // Пустая строка по умолчанию
	public string ActorFilter { get; set; } = ""; // Пустая строка по умолчанию
	public string SortBy { get; set; } = ""; // Пустая строка по умолчанию
}