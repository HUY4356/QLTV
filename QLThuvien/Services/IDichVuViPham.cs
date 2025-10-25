using System.Threading.Tasks;

namespace QLThuvien.Services
{
    public enum LoaiViPham
    {
        NoShowPhong,
        TreHanSach
    }

    public interface IDichVuViPham
    {
        Task GhiNhanViPhamAsync(int userId, LoaiViPham loaiViPham);
    }
}

