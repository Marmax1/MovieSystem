using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Movie
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public int? Directorid { get; set; }

    public int? Studioid { get; set; }

    public int? Releaseyear { get; set; }

    public double? Rating { get; set; }

    public string? Filepath { get; set; }

    public virtual Director? Director { get; set; }

    public virtual ICollection<Movieactor> Movieactors { get; set; } = new List<Movieactor>();

    public virtual ICollection<Moviegenre> Moviegenres { get; set; } = new List<Moviegenre>();

    public virtual Studio? Studio { get; set; }
}
