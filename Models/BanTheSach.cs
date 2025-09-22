using System;
using System.Collections.Generic;

namespace QL_ThuVien.Models;

public partial class BanTheSach
{
    public int Id { get; set; }

    public int DanhMucSachId { get; set; }

    public string MaVach { get; set; } = null!;

    public string TinhTrang { get; set; } = null!;

    public virtual DanhMucSach DanhMucSach { get; set; } = null!;

    public virtual ICollection<MuonTra> MuonTras { get; set; } = new List<MuonTra>();
}
