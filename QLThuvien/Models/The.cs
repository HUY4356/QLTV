using System;
using System.Collections.Generic;

namespace QLThuvien.Models;

public partial class The
{
    public int Id { get; set; }

    public string TheCode { get; set; } = null!;

    public int UserId { get; set; }

    public DateTime HanThe { get; set; }

    public string TrangThai { get; set; } = null!;

    public virtual ICollection<MuonTra> MuonTras { get; set; } = new List<MuonTra>();

    public virtual User User { get; set; } = null!;
}
