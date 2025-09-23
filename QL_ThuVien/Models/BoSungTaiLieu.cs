using System;
using System.Collections.Generic;

namespace QL_ThuVien.Models;

public partial class BoSungTaiLieu
{
    public int Id { get; set; }

    public string DeXuat { get; set; } = null!;

    public string XuLy { get; set; } = null!;

    public string CapNhat { get; set; } = null!;

    public string NoiDung { get; set; } = null!;

    public int? DanhMucSachId { get; set; }

    public virtual DanhMucSach? DanhMucSach { get; set; }
}
