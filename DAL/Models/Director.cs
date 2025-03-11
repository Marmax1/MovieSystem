using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Director
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Country { get; set; }

    public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();
}
