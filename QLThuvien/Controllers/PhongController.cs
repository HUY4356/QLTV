using Microsoft.AspNetCore.Mvc;
using QLThuvien.Services;
using QLThuvien.ViewModels;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using QLThuvien.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using QLThuvien.Models.Dto; // <-- THÊM DÒNG NÀY
using System.Collections.Generic;
using System;

namespace QLThuvien.Controllers
{
    [Route("DatPhong")]
    public class PhongController : Controller
    {
        private readonly IRoomService _roomService;
        private readonly ILogger<PhongController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ThuVienDbContext _context;

        public PhongController(
            IRoomService roomService,
            ILogger<PhongController> logger,
            UserManager<IdentityUser> userManager,
            ThuVienDbContext context)
        {
            _roomService = roomService;
            _logger = logger;
            _userManager = userManager;
            _context = context;
        }


        [Route("")]
        [Route("index")]
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("--- Bắt đầu xử lý action Index của PhongController ---");

            var viewModel = new PhongViewModel();
            // Sử dụng ?? new() để tránh lỗi null reference nếu service trả về null
            viewModel.DanhSachPhong = await _roomService.GetAllRoomsAsync() ?? new List<Phong>();

            // Lấy User ID (Thêm kiểm tra null chặt chẽ)
            if (User.Identity?.IsAuthenticated ?? false) // Kiểm tra User.Identity trước
            {
                var identityUser = await _userManager.GetUserAsync(User);
                // Kiểm tra cả identityUser và Email trước khi truy cập
                if (identityUser != null && identityUser.Email != null)
                {
                    // Tìm user bằng Email, đảm bảo không lỗi null
                    var appUser = await _context.Users.AsNoTracking()
                                                .FirstOrDefaultAsync(u => u.Email == identityUser.Email);
                    if (appUser != null)
                    {
                        viewModel.CurrentUserId = appUser.Id;
                        _logger.LogInformation($"Người dùng {identityUser.Email} (App User ID: {appUser.Id}) đã được xác thực.");
                    }
                    else
                    {
                        _logger.LogWarning($"Không tìm thấy App User với email: {identityUser.Email}");
                    }
                }
                else
                {
                    _logger.LogWarning("Identity User không tồn tại hoặc thiếu Email.");
                }
            }
            else
            {
                _logger.LogInformation("Người dùng chưa đăng nhập.");
            }


            // LẤY LỊCH ĐẶT PHÒNG
            if (viewModel.DanhSachPhong.Any())
            {
                var nowUtc = DateTime.UtcNow;
                foreach (var phong in viewModel.DanhSachPhong)
                {
                    // Dùng ToListAsync() để tránh lỗi null reference khi bookings là null
                    var bookings = await _context.DatPhongs
                        .Where(dp => dp.PhongId == phong.Id && dp.GioKetThuc > nowUtc)
                        .OrderBy(dp => dp.GioBatDau)
                        .Select(dp => new DatPhongDto(dp)) // Đã có using Dto
                        .ToListAsync() ?? new List<DatPhongDto>(); // Dùng ?? new()

                    // Thêm vào Dictionary
                    viewModel.LichDatPhong[phong.Id] = bookings;
                    _logger.LogInformation("Tìm thấy {Count} lượt đặt cho Phòng ID: {PhongId}", bookings.Count, phong.Id);
                }
            }

            return View(viewModel);
        }
    }
}

