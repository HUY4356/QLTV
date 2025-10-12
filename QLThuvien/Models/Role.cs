using System;
using System.Collections.Generic;

namespace QLThuvien.Models;

public partial class Role
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<HanMuc> HanMucs { get; set; } = new List<HanMuc>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
