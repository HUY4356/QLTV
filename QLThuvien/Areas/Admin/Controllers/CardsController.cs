using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLThuvien.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace QLThuvien.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,ThuThu")]
    public class CardsController : Controller
    {
        private readonly ThuVienDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public CardsController(ThuVienDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        // ==== ViewModel cấp thẻ theo Identity user ====
        public class CreateCardVM
        {
            [Required(ErrorMessage = "Chọn người dùng")]
            public string IdentityUserId { get; set; } = "";

            [Required, DataType(DataType.Date)]
            public DateTime HanThe { get; set; } = DateTime.Today.AddYears(1);

            [Required]
            public string TrangThai { get; set; } = "active";
        }

        // Danh sách + auto mark hết hạn (giữ nguyên nếu bạn đã có)
        public async Task<IActionResult> Index()
        {
            var today = DateTime.Today;
            var cards = await _db.Thes.Include(t => t.User)
                                      .OrderByDescending(t => t.Id)
                                      .ToListAsync();
            foreach (var c in cards.Where(x => x.HanThe.Date < today && x.TrangThai == "active"))
                c.TrangThai = "expired";
            if (_db.ChangeTracker.HasChanges()) await _db.SaveChangesAsync();

            return View(cards);
        }

        // GET: Cấp thẻ (lấy danh sách từ Identity)
        public async Task<IActionResult> Create()
        {
            ViewBag.IdentityUsers = await BuildIdentityUserListAsync();
            return View(new CreateCardVM());
        }

        // POST: Cấp thẻ
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCardVM m)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.IdentityUsers = await BuildIdentityUserListAsync();
                return View(m);
            }

            var iuser = await _userManager.FindByIdAsync(m.IdentityUserId);
            if (iuser is null)
            {
                ModelState.AddModelError("", "Không tìm thấy tài khoản Identity đã chọn.");
                ViewBag.IdentityUsers = await BuildIdentityUserListAsync();
                return View(m);
            }

            // Tìm hoặc tạo user domain theo Email
            var duser = await _db.Users.FirstOrDefaultAsync(x => x.Email == iuser.Email);
            if (duser is null)
            {
                var roles = await _userManager.GetRolesAsync(iuser);
                var roleName = roles.FirstOrDefault() ?? "DocGia";

                var role = await _db.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
                if (role == null)
                {
                    // fallback về DocGia
                    role = await _db.Roles.FirstOrDefaultAsync(r => r.Name == "DocGia");
                    if (role == null)
                    {
                        role = new Role { Name = "DocGia" };
                        _db.Roles.Add(role);
                        await _db.SaveChangesAsync();
                    }
                }

                // Lấy tên hiển thị từ claim "full_name"
                var claims = await _userManager.GetClaimsAsync(iuser);
                var fullName = claims.FirstOrDefault(c => c.Type == "full_name")?.Value
                               ?? iuser.UserName ?? iuser.Email!;

                duser = new User
                {
                    Fullname = fullName,
                    Email = iuser.Email!,
                    Phone = iuser.PhoneNumber,
                    RoleId = role.Id,
                    Password = "IDENTITY" 
                };
                _db.Users.Add(duser);
                await _db.SaveChangesAsync();
            }

            if (m.TrangThai == "active" &&
                await _db.Thes.AnyAsync(t => t.UserId == duser.Id && t.TrangThai == "active"))
            {
                ModelState.AddModelError("", "Người dùng đã có thẻ đang ở trạng thái 'active'.");
                ViewBag.IdentityUsers = await BuildIdentityUserListAsync();
                return View(m);
            }

            // Sinh mã thẻ <= 20 ký tự
            string code;
            do
            {
                code = $"TH{DateTime.UtcNow:yyMMdd}{Guid.NewGuid().ToString("N")[..6]}";
            } while (await _db.Thes.AnyAsync(t => t.TheCode == code));

            var card = new The
            {
                TheCode = code,
                UserId = duser.Id,
                HanThe = m.HanThe,
                TrangThai = m.TrangThai
            };
            _db.Thes.Add(card);
            await _db.SaveChangesAsync();

            TempData["Ok"] = "Cấp thẻ thành công.";
            return RedirectToAction(nameof(Index));
        }

        // AJAX: hiển thị hạn mức theo vai trò của Identity user
        [HttpGet]
        public async Task<IActionResult> LimitOfIdentityUser(string id)
        {
            var iuser = await _userManager.FindByIdAsync(id);
            if (iuser is null) return NotFound();

            var roles = await _userManager.GetRolesAsync(iuser);
            var roleName = roles.FirstOrDefault();
            if (roleName is null) return Json(new { ok = false });

            var hm = await _db.HanMucs.Include(x => x.Role)
                                      .FirstOrDefaultAsync(x => x.Role.Name == roleName);
            if (hm is null) return Json(new { ok = false });

            return Json(new
            {
                ok = true,
                role = hm.Role.Name,
                maxBooks = hm.MaxBooks,
                maxDays = hm.MaxDays,
                maxRenewals = hm.MaxRenewals,
                maxFines = hm.MaxFines
            });
        }

        // ===== helpers =====
        private async Task<List<object>> BuildIdentityUserListAsync()
        {
            var list = new List<object>();
            var identityUsers = await _userManager.Users.ToListAsync();
            foreach (var u in identityUsers)
            {
                var claims = await _userManager.GetClaimsAsync(u);
                var name = claims.FirstOrDefault(c => c.Type == "full_name")?.Value ?? u.UserName ?? u.Email!;
                list.Add(new { Id = u.Id, Name = $"{name} ({u.Email})" });
            }
            return list;
        }

        [HttpPost]
        public async Task<IActionResult> Block(int id)
        {
            var card = await _db.Thes.FindAsync(id);
            if (card == null) return NotFound();
            card.TrangThai = "blocked";
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExtendMonths(int id, int months = 1)
        {
            if (months < 1 || months > 60) months = 1; // chặn giá trị bất hợp lý

            var card = await _db.Thes.FindAsync(id);
            if (card == null) return NotFound();

            card.HanThe = card.HanThe.AddMonths(months);

            // Nếu thẻ đang hết hạn mà sau gia hạn ngày mới >= hôm nay thì chuyển về active
            if (card.TrangThai == "expired" && card.HanThe.Date >= DateTime.Today)
                card.TrangThai = "active";

            await _db.SaveChangesAsync();
            TempData["Ok"] = $"Đã gia hạn thêm {months} tháng.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Extend(int id)
        {
            var card = await _db.Thes.FindAsync(id);
            if (card == null) return NotFound();
            card.HanThe = card.HanThe.AddYears(1);
            if (card.TrangThai == "expired") card.TrangThai = "active";
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // MỞ KHÓA thẻ
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unlock(int id)
        {
            var card = await _db.Thes.FindAsync(id);
            if (card == null) return NotFound();

            // Mở khóa: nếu còn hạn → active, nếu đã quá hạn → expired
            card.TrangThai = (card.HanThe.Date < DateTime.Today) ? "expired" : "active";

            await _db.SaveChangesAsync();
            TempData["Ok"] = card.TrangThai == "active"
                ? "Đã mở khóa thẻ."
                : "Đã mở khóa thẻ nhưng thẻ đang hết hạn.";
            return RedirectToAction(nameof(Index));
        }
    }
}
