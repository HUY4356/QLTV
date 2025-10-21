using QLThuvien.Models;
using QLThuvien.Models.Dto;
using System.Threading.Tasks;

namespace QLThuvien.Services
{
    public interface IDichVuDatPhong
    {
        // Sửa lại phương thức này
        Task<(bool ThanhCong, string ThongBao, DatPhong? DatPhongMoi)> TaoDatPhongAsync(YeuCauDatPhongDto yeuCau, int userId);
    }
}
