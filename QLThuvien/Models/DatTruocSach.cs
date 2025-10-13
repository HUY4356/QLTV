using System;
using System.Collections.Generic;

namespace QLThuvien.Models;

public partial class DatTruocSach
{
    public int Id { get; set; }

    public int DanhMucSachId { get; set; }

    public int UserId { get; set; }

    public DateTime NgayDat { get; set; }

    public string TrangThai { get; set; } = null!;

    public virtual DanhMucSach DanhMucSach { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
