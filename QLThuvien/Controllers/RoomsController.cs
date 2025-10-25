using Microsoft.AspNetCore.Mvc;
using QLThuvien.Models;
using QLThuvien.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QLThuvien.Controllers
{
    [Route("api/[controller]")] // Đường dẫn sẽ là /api/rooms
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomsController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        // GET: /api/rooms (Lấy tất cả phòng)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Phong>>> GetRooms()
        {
            var rooms = await _roomService.GetAllRoomsAsync();
            return Ok(rooms);
        }

        // Dùng để frontend gọi khi muốn cập nhật trạng thái
        public class UpdateStatusRequest
        {
            public RoomStatus NewStatus { get; set; }
        }

        // GET: /api/rooms/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Phong>> GetRoom(int id)
        {
            var room = await _roomService.GetRoomByIdAsync(id);

            // Kiểm tra xem room có bị null không
            if (room == null)
            {
                // Nếu không tìm thấy, trả về lỗi 404 Not Found
                return NotFound();
            }

            // Nếu tìm thấy, trả về 200 OK và đối tượng room
            return Ok(room);
        }
    }
}