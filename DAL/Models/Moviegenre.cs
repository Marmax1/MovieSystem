using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Moviegenre
{
    public int Id { get; set; }

    public int? Movieid { get; set; }

    public int? Genreid { get; set; }

    public virtual Genre? Genre { get; set; }

    public virtual Movie? Movie { get; set; }
}
