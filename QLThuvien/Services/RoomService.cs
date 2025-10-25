using Microsoft.EntityFrameworkCore;
using QLThuvien.Data; // Thư mục chứa ThuVienDbContext
using QLThuvien.Models; // Thư mục chứa Phong, RoomStatus...
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QLThuvien.Services
{
    public class RoomService : IRoomService
    {
        private readonly ThuVienDbContext _context;

        public RoomService(ThuVienDbContext context)
        {
            _context = context;
        }

        public async Task<List<Phong>> GetAllRoomsAsync()
        {
            return await _context.Phongs.ToListAsync();
        }

        public async Task<Phong?> GetRoomByIdAsync(int id)
        {
            return await _context.Phongs.FindAsync(id);
        }

        public async Task<bool> UpdateRoomStatusAsync(int id, RoomStatus newStatus)
        {
            var room = await _context.Phongs.FindAsync(id);
            if (room == null)
            {
                return false; // Không tìm thấy
            }

            room.TrangThai = newStatus;
            await _context.SaveChangesAsync();
            return true;
        }

        // Hàm tạo 6 phòng mẫu (theo yêu cầu frontend)
        public async Task SeedRoomsAsync()
        {
            // Chỉ chạy nếu bảng Phong chưa có dữ liệu
            if (!await _context.Phongs.AnyAsync())
            {
                var sampleRooms = new List<Phong>
        {
            new Phong { TenPhong = "Phòng Cá Nhân 01", DienTich = 10, SucChua = 1, LoaiPhong = RoomType.CaNhan, TrangThai = RoomStatus.Trong },
            new Phong { TenPhong = "Phòng Nhóm A", DienTich = 20, SucChua = 6, LoaiPhong = RoomType.Nhom, TrangThai = RoomStatus.Trong },
            // Sửa lại thành Trong, để Background Service tự quyết định
            new Phong { TenPhong = "Phòng Thuê 01", DienTich = 15, SucChua = 4, LoaiPhong = RoomType.Thue, TrangThai = RoomStatus.Trong },
            new Phong { TenPhong = "Phòng Riêng 01", DienTich = 12, SucChua = 2, LoaiPhong = RoomType.Rieng, TrangThai = RoomStatus.Trong },
            // Có thể giữ lại BaoTri nếu bạn muốn test trạng thái này
            new Phong { TenPhong = "Bàn Cửa Sổ 01", DienTich = 8, SucChua = 1, LoaiPhong = RoomType.BanCuaSo, TrangThai = RoomStatus.BaoTri },
            new Phong { TenPhong = "Phòng VIP 01", DienTich = 25, SucChua = 8, LoaiPhong = RoomType.Vip, TrangThai = RoomStatus.Trong }
        };

                await _context.Phongs.AddRangeAsync(sampleRooms);
                await _context.SaveChangesAsync();
            }
        }
    }
}