using QLThuvien.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QLThuvien.Services
{
    public interface IRoomService
    {
        Task<List<Phong>> GetAllRoomsAsync();
        Task<Phong?> GetRoomByIdAsync(int id);
        Task<bool> UpdateRoomStatusAsync(int id, RoomStatus newStatus);

        // (Tùy chọn) Thêm hàm này để tạo 6 phòng mẫu
        Task SeedRoomsAsync();
    }
}