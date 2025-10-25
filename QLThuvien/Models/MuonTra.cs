using System;
using System.Collections.Generic;

namespace QLThuvien.Models;

public partial class MuonTra
{
    public int Id { get; set; }

    public int TheId { get; set; }

    public int BanTheSachId { get; set; }

    public DateTime NgayMuon { get; set; }

    public DateTime DueDate { get; set; }

    public DateTime? NgayTra { get; set; }

    public int SoLanGiaHan { get; set; }

    public decimal? TienPhat { get; set; }

    public string TrangThai { get; set; } = null!;

    public virtual BanTheSach BanTheSach { get; set; } = null!;

    public virtual The The { get; set; } = null!;
}
