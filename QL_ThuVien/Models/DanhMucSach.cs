using System;
using System.Collections.Generic;

namespace QL_ThuVien.Models;

public partial class DanhMucSach
{
    public int Id { get; set; }

    public int NhomSachId { get; set; }

    public string MaSach { get; set; } = null!;

    public string TenSach { get; set; } = null!;

    public string TacGia { get; set; } = null!;

    public decimal DonGia { get; set; }

    public int Slton { get; set; }

    public string ViTriKe { get; set; } = null!;

    public virtual ICollection<BanTheSach> BanTheSaches { get; set; } = new List<BanTheSach>();

    public virtual ICollection<BaoCaoVongQuaySach> BaoCaoVongQuaySaches { get; set; } = new List<BaoCaoVongQuaySach>();

    public virtual ICollection<BoSungTaiLieu> BoSungTaiLieus { get; set; } = new List<BoSungTaiLieu>();

    public virtual ICollection<DatTruocSach> DatTruocSaches { get; set; } = new List<DatTruocSach>();

    public virtual NhomSach NhomSach { get; set; } = null!;
}
