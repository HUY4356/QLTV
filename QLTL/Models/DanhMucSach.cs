using System.ComponentModel.DataAnnotations.Schema;

namespace QLTV_Backend.Models
{
    public class DanhMucSach
    {
        public int Id { get; set; }

        // Khóa ngoại (theo convention .NET: <EntityName>Id)
        public int NhomSachId { get; set; }

        // Navigation property
        [ForeignKey("NhomSachId")]
        public virtual NhomSach NhomSach { get; set; }

        public string MaSach { get; set; }
        public string TenSach { get; set; }
        public string TacGia { get; set; }
        public string DonGia { get; set; }

        // các liên kết khác...
    }
}
