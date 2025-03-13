public class MovieViewModel
{
	public int Id { get; set; }
	public string Title { get; set; }
	public int ReleaseYear { get; set; }
	public double Rating { get; set; }
	public string DirectorName { get; set; }
	public string StudioName { get; set; }
	public List<string> Genres { get; set; } = new();
	public List<string> Actors { get; set; } = new();
}