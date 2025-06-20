﻿using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Genre
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Moviegenre> Moviegenres { get; set; } = new List<Moviegenre>();
}
