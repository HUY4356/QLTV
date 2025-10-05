using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QL_ThuVien.Models;

public partial class User
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Họ tên không được để trống")]
    public string Fullname { get; set; } = null!;

    [Required(ErrorMessage = "Email không được để trống")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    public string Email { get; set; } = null!;

    [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
    public string? Phone { get; set; }

    [Required(ErrorMessage = "Mật khẩu không được để trống")]
    public string Password { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    [Required(ErrorMessage = "Vai trò không được để trống")]
    public int RoleId { get; set; }

    public virtual ICollection<DatPhong> DatPhongs { get; set; } = new List<DatPhong>();

    public virtual ICollection<DatTruocSach> DatTruocSaches { get; set; } = new List<DatTruocSach>();

    public virtual Role? Role { get; set; }

    public virtual ICollection<The> Thes { get; set; } = new List<The>();

    public virtual ICollection<ViPhamUser> ViPhamUsers { get; set; } = new List<ViPhamUser>();
}
