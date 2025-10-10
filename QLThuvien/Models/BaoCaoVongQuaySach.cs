using System;
using System.Collections.Generic;

namespace QLThuvien.Models;

public partial class BaoCaoVongQuaySach
{
    public int Id { get; set; }

    public int DanhMucSachId { get; set; }

    public int ThongKeLuotMuon { get; set; }

    public DateTime ThoiGianBaoCao { get; set; }

    public virtual DanhMucSach DanhMucSach { get; set; } = null!;
}
