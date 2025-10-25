using System.Threading.Tasks;

namespace QLThuvien.Services
{
    public interface IDichVuMuonTra
    {
        /// <summary>
        /// Xử lý việc trả sách, kiểm tra trễ hạn và tính phạt.
        /// </summary>
        Task<(bool ThanhCong, string ThongBao)> TraSachAsync(int muonTraId);
        // ... (Các hàm khác nếu có)
    }
}

