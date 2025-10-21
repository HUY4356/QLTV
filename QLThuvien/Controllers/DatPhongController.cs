using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLThuvien.Models;
using QLThuvien.Models.Dto;
using QLThuvien.Services;
using System.Threading.Tasks;

namespace QLThuvien.Controllers
{
    [Route("api/datphong")]
    [ApiController]
    [Authorize] // Yêu cầu người dùng phải đăng nhập để truy cập API này
    public class DatPhongController : ControllerBase
    {
        private readonly IDichVuDatPhong _dichVuDatPhong;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ThuVienDbContext _context;

        public DatPhongController(
            IDichVuDatPhong dichVuDatPhong,
            UserManager<IdentityUser> userManager,
            ThuVienDbContext context)
        {
            _dichVuDatPhong = dichVuDatPhong;
            _userManager = userManager;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] YeuCauDatPhongDto yeuCau)
        {
            // --- LẤY USER ID TỪ SERVER-SIDE ---
            var identityUser = await _userManager.GetUserAsync(User);
            if (identityUser == null)
            {
                return Unauthorized(new { message = "Phiên đăng nhập không hợp lệ." });
            }

            var appUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == identityUser.Email);
            if (appUser == null)
            {
                return NotFound(new { message = "Không tìm thấy thông tin người dùng trong hệ thống." });
            }
            // ------------------------------------

            var (thanhCong, thongBao, datPhongMoi) = await _dichVuDatPhong.TaoDatPhongAsync(yeuCau, appUser.Id);

            if (!thanhCong)
            {
                // Sử dụng switch để trả về mã lỗi HTTP phù hợp
                switch (thongBao)
                {
                    case var s when s.Contains("không tồn tại"):
                        return NotFound(new { message = thongBao });
                    case var s when s.Contains("trùng với lịch khác"):
                        return Conflict(new { message = thongBao }); // 409 Conflict
                    default:
                        return BadRequest(new { message = thongBao }); // 400 Bad Request
                }
            }

            return CreatedAtAction(nameof(CreateBooking), new { id = datPhongMoi.Id }, datPhongMoi);
        }
    }
}