using Microsoft.EntityFrameworkCore;
using QLThuvien.Models;
using QLThuvien.Models.Dto;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace QLThuvien.Services
{
    public class DichVuDatPhong : IDichVuDatPhong
    {
        private readonly ThuVienDbContext _context;

        public DichVuDatPhong(ThuVienDbContext context)
        {
            _context = context;
        }

        public async Task<(bool ThanhCong, string ThongBao, DatPhong? DatPhongMoi)> TaoDatPhongAsync(YeuCauDatPhongDto yeuCau, int userId)
        {
            // 1. Kiểm tra User có tồn tại không
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return (false, "Người dùng không tồn tại.", null);
            }

            // 2. Kiểm tra Phòng có tồn tại không
            var phong = await _context.Phongs.FindAsync(yeuCau.PhongId);
            if (phong == null)
            {
                return (false, "Phòng không tồn tại.", null);
            }

            // 3. Kiểm tra phòng có đang bảo trì không
            if (phong.TrangThai == RoomStatus.BaoTri)
            {
                return (false, "Phòng đang được bảo trì, không thể đặt.", null);
            }

            // 4. Kiểm tra lịch có bị trùng không
            var isOverlapping = await _context.DatPhongs
                .AnyAsync(dp =>
                    dp.PhongId == yeuCau.PhongId &&
                    dp.GioBatDau < yeuCau.GioKetThuc && // Lượt đặt cũ bắt đầu trước khi lượt đặt mới kết thúc
                    dp.GioKetThuc > yeuCau.GioBatDau    // Lượt đặt cũ kết thúc sau khi lượt đặt mới bắt đầu
                );

            if (isOverlapping)
            {
                return (false, "Lịch bạn chọn đã có người đặt hoặc trùng với lịch khác.", null);
            }

            // 5. Nếu mọi thứ hợp lệ, tạo lượt đặt phòng mới
            var newBooking = new DatPhong
            {
                UserId = userId, // Lấy từ tham số
                PhongId = yeuCau.PhongId,
                GioBatDau = yeuCau.GioBatDau,
                GioKetThuc = yeuCau.GioKetThuc,
                NgayDat = DateTime.Now,
                TrangThai = "DaXacNhan" // Hoặc "DangChoDuyet" tùy theo quy trình của bạn
            };

            _context.DatPhongs.Add(newBooking);
            await _context.SaveChangesAsync();

            return (true, "Đặt phòng thành công!", newBooking);
        }
    }
}
