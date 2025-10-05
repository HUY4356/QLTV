using System;
using System.Collections.Generic;

namespace QL_ThuVien.Models;

public partial class NhomSach
{
    public int Id { get; set; }

    public string MaNhom { get; set; } = null!;

    public string TenNhom { get; set; } = null!;

    public virtual ICollection<DanhMucSach> DanhMucSaches { get; set; } = new List<DanhMucSach>();
}
