using QLThuvien.Models;
using System.Collections.Generic;

namespace QLThuvien.ViewModels
{
    public class PhongViewModel
    {
        public List<Phong> DanhSachPhong { get; set; } = new List<Phong>();

        // THÊM THUỘC TÍNH NÀY
        // Dùng để lưu ID của user đang đăng nhập (từ bảng User, không phải Identity)
        public int? CurrentUserId { get; set; }
    }
}