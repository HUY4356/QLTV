using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLThuvien.Models;
using QLThuvien.Models.Dto; // <-- ĐẢM BẢO CÓ DÒNG NÀY
using QLThuvien.Services;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QLThuvien.Controllers
{
    [Route("api/datphong")]
    [ApiController]
    [Authorize]
    public class DatPhongController : ControllerBase
    {
        // ...(Constructor và các Action như trước)...
        private readonly IDichVuDatPhong _dichVuDatPhong;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ThuVienDbContext _context;
        private readonly ILogger<DatPhongController> _logger;

        public DatPhongController(
            IDichVuDatPhong dichVuDatPhong,
            UserManager<IdentityUser> userManager,
            ThuVienDbContext context,
            ILogger<DatPhongController> logger)
        {
            _dichVuDatPhong = dichVuDatPhong;
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] YeuCauDatPhongDto yeuCau) // Sử dụng YeuCauDatPhongDto
        {
            try
            {
                _logger.LogInformation("Nhận yêu cầu đặt phòng API: {@YeuCau}", yeuCau);

                var identityUser = await _userManager.GetUserAsync(User);
                if (identityUser == null || identityUser.Email == null)
                {
                    _logger.LogWarning("API CreateBooking: Không tìm thấy Identity User hoặc Email null.");
                    return Unauthorized(new { message = "Phiên đăng nhập không hợp lệ hoặc thiếu thông tin email." });
                }

                // Dùng AsNoTracking nếu chỉ đọc dữ liệu
                var appUser = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == identityUser.Email);
                if (appUser == null)
                {
                    _logger.LogWarning("API CreateBooking: Không tìm thấy App User với email {Email}.", identityUser.Email);
                    return NotFound(new { message = $"Không tìm thấy thông tin người dùng ứng dụng với email {identityUser.Email}." });
                }
                _logger.LogInformation("API CreateBooking: User ID là {UserId}", appUser.Id);

                var (thanhCong, thongBao, datPhongMoi) = await _dichVuDatPhong.TaoDatPhongAsync(yeuCau, appUser.Id);

                if (!thanhCong)
                {
                    _logger.LogWarning("API CreateBooking: Dịch vụ đặt phòng trả về lỗi: {ErrorMessage}", thongBao);
                    switch (thongBao)
                    {
                        case var s when s.Contains("không tồn tại"): return NotFound(new { message = thongBao });
                        case var s when s.Contains("trùng với lịch khác"): return Conflict(new { message = thongBao });
                        case var s when s.Contains("bảo trì"): return BadRequest(new { message = thongBao });
                        case var s when s.Contains("thời gian"): return BadRequest(new { message = thongBao });
                        default: return BadRequest(new { message = thongBao });
                    }
                }

                if (datPhongMoi == null)
                {
                    _logger.LogError("API CreateBooking: Service báo thành công nhưng datPhongMoi là null.");
                    return StatusCode(500, new { message = "Lỗi không xác định sau khi tạo đặt phòng." });
                }

                // Chuyển sang DTO trước khi trả về
                var datPhongDtoResult = new DatPhongDto(datPhongMoi);
                _logger.LogInformation("API CreateBooking: Đặt phòng thành công, trả về DTO cho ID mới: {BookingId}", datPhongMoi.Id);
                // Giả sử có action GetBookingById
                return CreatedAtAction(nameof(GetBookingById), new { id = datPhongDtoResult.Id }, datPhongDtoResult);
                // Hoặc return Created($"/api/datphong/{datPhongDtoResult.Id}", datPhongDtoResult);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "LỖI NGHIÊM TRỌNG (500) xảy ra trong API CreateBooking");
                return StatusCode(500, new { message = "Đã xảy ra lỗi không mong muốn phía máy chủ." });
            }
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous] // Hoặc [Authorize]
        public async Task<ActionResult<DatPhongDto>> GetBookingById(int id) // Trả về DatPhongDto
        {
            _logger.LogInformation("Nhận yêu cầu lấy thông tin đặt phòng ID: {Id}", id);
            var booking = await _context.DatPhongs
                                       .AsNoTracking() // Dùng AsNoTracking nếu chỉ đọc
                                       .FirstOrDefaultAsync(dp => dp.Id == id);

            if (booking == null)
            {
                _logger.LogWarning("API GetBookingById: Không tìm thấy đặt phòng ID: {Id}", id);
                return NotFound();
            }

            var bookingDto = new DatPhongDto(booking); // Chuyển đổi sang DTO
            return Ok(bookingDto);
        }

        [HttpGet("lich/{phongId:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<DatPhongDto>>> GetBookingsForRoom(int phongId, [FromQuery] DateTime? fromDate = null)
        {
            _logger.LogInformation("Nhận yêu cầu lấy lịch đặt cho Phòng ID: {PhongId}", phongId);
            var phongExists = await _context.Phongs.AnyAsync(p => p.Id == phongId);
            if (!phongExists)
            {
                _logger.LogWarning("API GetBookings: Không tìm thấy Phòng ID: {PhongId}", phongId);
                return NotFound(new { message = "Phòng không tồn tại." });
            }
            var nowUtc = DateTime.UtcNow;
            var startDateUtc = fromDate.HasValue
               ? DateTime.SpecifyKind(fromDate.Value.Date, DateTimeKind.Local).ToUniversalTime()
               : nowUtc;

            // Đảm bảo trả về List<DatPhongDto>
            var bookings = await _context.DatPhongs
                .Where(dp => dp.PhongId == phongId && dp.GioKetThuc > startDateUtc)
                .OrderBy(dp => dp.GioBatDau)
                .Select(dp => new DatPhongDto(dp)) // Đã có using Dto
                .ToListAsync();

            _logger.LogInformation("API GetBookings: Tìm thấy {Count} lượt đặt cho Phòng ID: {PhongId} từ {StartDate}", bookings.Count, phongId, startDateUtc);
            return Ok(bookings);
        }
    }
}

