using System;
using System.Collections.Generic;

namespace QL_ThuVien.Models;

public partial class ViPhamUser
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int? CountNoShow { get; set; }

    public int? CountTreHan { get; set; }

    public DateTime? LastUpdated { get; set; }

    public virtual User User { get; set; } = null!;
}
