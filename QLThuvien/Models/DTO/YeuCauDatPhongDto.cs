using System;
using System.ComponentModel.DataAnnotations; // Thêm using này

namespace QLThuvien.Models.Dto
{
    // Lớp này đại diện cho dữ liệu gửi từ client khi đặt phòng
    public class YeuCauDatPhongDto
    {
        [Required(ErrorMessage = "Mã phòng là bắt buộc.")]
        public int PhongId { get; set; }

        [Required(ErrorMessage = "Thời gian bắt đầu là bắt buộc.")]
        public DateTime GioBatDau { get; set; }

        [Required(ErrorMessage = "Thời gian kết thúc là bắt buộc.")]
        public DateTime GioKetThuc { get; set; }
    }
}

