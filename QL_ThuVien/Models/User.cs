using System;
using System.Collections.Generic;

namespace QL_ThuVien.Models;

public partial class User
{
    public int Id { get; set; }

    public string Fullname { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public string Password { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public int RoleId { get; set; }

    public virtual ICollection<DatPhong> DatPhongs { get; set; } = new List<DatPhong>();

    public virtual ICollection<DatTruocSach> DatTruocSaches { get; set; } = new List<DatTruocSach>();

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<The> Thes { get; set; } = new List<The>();

    public virtual ICollection<ViPhamUser> ViPhamUsers { get; set; } = new List<ViPhamUser>();
}
