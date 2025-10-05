using System;
using System.Collections.Generic;

namespace QL_ThuVien.Models;

public partial class Phong
{
    public int Id { get; set; }

    public string TenPhong { get; set; } = null!;

    public decimal DienTich { get; set; }

    public int SucChua { get; set; }

    public virtual ICollection<DatPhong> DatPhongs { get; set; } = new List<DatPhong>();
}
