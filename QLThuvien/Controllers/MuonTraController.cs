using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QLThuvien.Services; // Using Services
using System;
using System.Threading.Tasks;

namespace QLThuvien.Controllers
{
    [Route("api/muontra")] // Đường dẫn API: /api/muontra
    [ApiController]
    [Authorize(Roles = "Admin,ThuThu")] // Chỉ Admin hoặc Thủ thư mới được trả sách
    public class MuonTraController : ControllerBase
    {
        private readonly IDichVuMuonTra _dichVuMuonTra;
        private readonly ILogger<MuonTraController> _logger;

        public MuonTraController(IDichVuMuonTra dichVuMuonTra, ILogger<MuonTraController> logger)
        {
            _dichVuMuonTra = dichVuMuonTra;
            _logger = logger;
        }

        /// <summary>
        /// Đánh dấu một lượt mượn sách là đã trả, kiểm tra trễ hạn và tính phạt.
        /// </summary>
        /// <param name="id">ID của bản ghi trong bảng MuonTra.</param>
        /// <returns>Kết quả xử lý.</returns>
        // POST /api/muontra/{id}/return
        [HttpPost("{id:int}/return")]
        public async Task<IActionResult> ReturnBook(int id)
        {
            _logger.LogInformation("Nhận yêu cầu API trả sách cho MuonTra ID: {MuonTraId}", id);
            try
            {
                var (thanhCong, thongBao) = await _dichVuMuonTra.TraSachAsync(id);

                if (!thanhCong)
                {
                    _logger.LogWarning("API ReturnBook: Service trả về lỗi: {ErrorMessage}", thongBao);
                    // Trả về lỗi phù hợp
                    if (thongBao.Contains("Không tìm thấy"))
                        return NotFound(new { message = thongBao });
                    else if (thongBao.Contains("đã được trả"))
                        return Conflict(new { message = thongBao });
                    else
                        return BadRequest(new { message = thongBao });
                }

                _logger.LogInformation("API ReturnBook: Trả sách thành công cho MuonTra ID: {MuonTraId}", id);
                return Ok(new { message = thongBao }); // Trả về 200 OK cùng thông báo
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi không mong muốn trong API ReturnBook (Trả sách) cho MuonTra ID: {MuonTraId}.", id);
                return StatusCode(500, new { message = "Đã xảy ra lỗi không mong muốn phía máy chủ." });
            }
        }

        // --- CÁC ACTION KHÁC CÓ THỂ THÊM ---
        // POST /api/muontra -> Tạo lượt mượn mới
        // POST /api/muontra/{id}/renew -> Gia hạn sách
        // GET /api/muontra/user/{userId} -> Lấy sách đang mượn của user
        // ------------------------------------
    }
}

