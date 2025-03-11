using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Movieactor
{
    public int Id { get; set; }

    public int? Movieid { get; set; }

    public int? Actorid { get; set; }

    public virtual Actor? Actor { get; set; }

    public virtual Movie? Movie { get; set; }
}
