using QLThuvien.Models;
using QLThuvien.Models.Dto; // <-- THÊM DÒNG NÀY
using System.Collections.Generic;

namespace QLThuvien.ViewModels
{
    public class PhongViewModel
    {
        public List<Phong> DanhSachPhong { get; set; } = new List<Phong>();
        public int? CurrentUserId { get; set; }

        // Sử dụng DatPhongDto (đã có using)
        public Dictionary<int, List<DatPhongDto>> LichDatPhong { get; set; } = new Dictionary<int, List<DatPhongDto>>();
    }
}

