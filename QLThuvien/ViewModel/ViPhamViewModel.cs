using QLThuvien.Models; // Cần using Models nếu ViPhamDetail cần tham chiếu
using System; // Cần using DateTime
using System.Collections.Generic;

namespace QLThuvien.ViewModel
{
    // ViewModel để hiển thị danh sách vi phạm cho Admin/Thủ thư
    public class ViPhamViewModel
    {
        public List<ViPhamDetail> DanhSachViPham { get; set; } = new List<ViPhamDetail>();
        public string? SearchTerm { get; set; } // Để tìm kiếm theo tên hoặc email
        // Có thể thêm các thuộc tính phân trang sau này
    }

    // Lớp phụ để chứa thông tin chi tiết hơn cho mỗi vi phạm
    public class ViPhamDetail
    {
        public int ViPhamId { get; set; }
        public int UserId { get; set; }
        public string? UserName { get; set; } // Tên đầy đủ người dùng
        public string? UserEmail { get; set; } // Email người dùng
        public int SoLanNoShow { get; set; }
        public int SoLanTreHan { get; set; }
        public DateTime? CapNhatCuoi { get; set; } // Thời điểm vi phạm cuối cùng
    }
}

