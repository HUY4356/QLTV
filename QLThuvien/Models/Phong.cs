using System;
using System.Collections.Generic;

namespace QLThuvien.Models;

public partial class Phong
{
    public int Id { get; set; }

    public string TenPhong { get; set; } = null!;

    public decimal DienTich { get; set; }

    public int SucChua { get; set; }

    public RoomType LoaiPhong { get; set; }

    public RoomStatus TrangThai { get; set; }

    public virtual ICollection<DatPhong> DatPhongs { get; set; } = new List<DatPhong>();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}



