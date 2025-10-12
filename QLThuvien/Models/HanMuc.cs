using System;
using System.Collections.Generic;

namespace QLThuvien.Models;

public partial class HanMuc
{
    public int Id { get; set; }

    public int RoleId { get; set; }

    public int MaxBooks { get; set; }

    public int MaxDays { get; set; }

    public int MaxRenewals { get; set; }

    public decimal MaxFines { get; set; }

    public virtual Role Role { get; set; } = null!;
}
