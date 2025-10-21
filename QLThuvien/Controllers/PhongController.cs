using Microsoft.AspNetCore.Mvc;
using QLThuvien.Services;
using QLThuvien.ViewModels;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity; // Thêm using cho Identity
using QLThuvien.Models; // Thêm using cho DbContext
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace QLThuvien.Controllers
{
    [Route("DatPhong")]
    public class PhongController : Controller
    {
        private readonly IRoomService _roomService;
        private readonly ILogger<PhongController> _logger;
        private readonly UserManager<IdentityUser> _userManager; // Dùng để lấy thông tin user đăng nhập
        private readonly ThuVienDbContext _context; // Dùng để tìm user trong bảng của bạn

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
            viewModel.DanhSachPhong = await _roomService.GetAllRoomsAsync() ?? new List<Phong>();

            // --- LOGIC LẤY USER ID TỰ ĐỘNG ---
            if (User.Identity.IsAuthenticated)
            {
                // Lấy thông tin user của hệ thống Identity
                var identityUser = await _userManager.GetUserAsync(User);
                if (identityUser != null)
                {
                    // Dựa vào email, tìm user tương ứng trong bảng User của bạn
                    var appUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == identityUser.Email);
                    if (appUser != null)
                    {
                        // Gán ID (kiểu int) của user vào ViewModel
                        viewModel.CurrentUserId = appUser.Id;
                        _logger.LogInformation($"Người dùng {identityUser.Email} (App User ID: {appUser.Id}) đã được xác thực.");
                    }
                    else
                    {
                        _logger.LogWarning($"Không tìm thấy user ứng dụng với email: {identityUser.Email}");
                    }
                }
            }
            // ------------------------------------

            return View(viewModel);
        }
    }
}