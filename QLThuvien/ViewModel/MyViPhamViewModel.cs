using System;

namespace QLThuvien.ViewModels
{
    // ViewModel để hiển thị vi phạm cho người dùng đang đăng nhập
    public class MyViPhamViewModel
    {
        public int SoLanNoShow { get; set; }
        public int SoLanTreHan { get; set; }
        public DateTime? CapNhatCuoi { get; set; }

        // Thuộc tính tiện ích để kiểm tra có vi phạm nào không
        public bool HasViolations => SoLanNoShow > 0 || SoLanTreHan > 0;
    }
}

