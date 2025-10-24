using QLTV_Backend.Models;
using System.ComponentModel.DataAnnotations.Schema;

public class BaoCaoVongQuaySach
{
    public int Id { get; set; }

    // Khóa ngoại
    public int DanhMucSachId { get; set; }

    // Navigation property
    [ForeignKey("DanhMucSachId")]
    public virtual DanhMucSach DanhMucSach { get; set; }

    public string ThongKeLuotMuon { get; set; }
}
