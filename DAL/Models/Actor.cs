using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Actor
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateOnly? Birthdate { get; set; }

    public string? Country { get; set; }

    public virtual ICollection<Movieactor> Movieactors { get; set; } = new List<Movieactor>();
}
