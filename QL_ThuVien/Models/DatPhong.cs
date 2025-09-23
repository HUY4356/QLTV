using System;
using System.Collections.Generic;

namespace QL_ThuVien.Models;

public partial class DatPhong
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int PhongId { get; set; }

    public DateTime NgayDat { get; set; }

    public DateTime GioBatDau { get; set; }

    public DateTime GioKetThuc { get; set; }

    public string TrangThai { get; set; } = null!;

    public virtual Phong Phong { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
