using System;
using System.Collections.Generic;

namespace QLThuvien.Models;

public partial class ThamSoHeThong
{
    public int Id { get; set; }

    public string Ten { get; set; } = null!;

    public string GiaTri { get; set; } = null!;
}
