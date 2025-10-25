using System;
using QLThuvien.Models; // <-- ĐẢM BẢO CÓ USING NÀY

namespace QLThuvien.Models.Dto
{
    public class DatPhongDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PhongId { get; set; }
        public DateTime NgayDat { get; set; }
        public DateTime GioBatDau { get; set; }
        public DateTime GioKetThuc { get; set; }
        // Thêm = null! để giải quyết lỗi nullability
        public string TrangThai { get; set; } = null!;

        // Constructor để chuyển đổi
        public DatPhongDto(DatPhong entity)
        {
            Id = entity.Id;
            UserId = entity.UserId;
            PhongId = entity.PhongId;
            NgayDat = entity.NgayDat;
            GioBatDau = entity.GioBatDau;
            GioKetThuc = entity.GioKetThuc;
            TrangThai = entity.TrangThai; // Đã có = null! ở trên
        }

        // Constructor không tham số
        public DatPhongDto() { }
    }
}

