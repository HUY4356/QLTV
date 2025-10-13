using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLThuvien.Models;

namespace QLThuvien.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,ThuThu")]
    public class CardsController : Controller
    {
        private readonly ThuVienDbContext _db;
        public CardsController(ThuVienDbContext db) => _db = db;

        // Danh sách thẻ (auto đánh dấu hết hạn nếu quá ngày)
        public async Task<IActionResult> Index()
        {
            var today = DateTime.Today;
            var cards = await _db.Thes
                .Include(t => t.User)
                .OrderByDescending(t => t.Id)
                .ToListAsync();

            foreach (var c in cards.Where(x => x.HanThe.Date < today && x.TrangThai == "active"))
                c.TrangThai = "expired";
            if (_db.ChangeTracker.HasChanges()) await _db.SaveChangesAsync();

            return View(cards);
        }

        // GET: Cấp thẻ
        public async Task<IActionResult> Create()
        {
            ViewBag.Users = await _db.Users
                .Select(u => new { u.Id, Name = u.Fullname + " (" + u.Email + ")" })
                .ToListAsync();

            return View(new The
            {
                TrangThai = "active",
                HanThe = DateTime.Today.AddYears(1)
            });
        }

        // POST: Cấp thẻ
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(The model)
        {
            // Validate người dùng
            if (!await _db.Users.AnyAsync(u => u.Id == model.UserId))
                ModelState.AddModelError(nameof(model.UserId), "Người dùng không tồn tại.");

            // Không cấp 2 thẻ active cho cùng user
            var hasActive = await _db.Thes.AnyAsync(t => t.UserId == model.UserId && t.TrangThai == "active");
            if (hasActive) ModelState.AddModelError("", "Người dùng đã có thẻ đang 'active'.");

            // Ràng buộc trạng thái
            var allowed = new[] { "active", "expired", "blocked" };
            if (!allowed.Contains(model.TrangThai)) model.TrangThai = "active";

            if (!ModelState.IsValid)
            {
                ViewBag.Users = await _db.Users
                    .Select(u => new { u.Id, Name = u.Fullname + " (" + u.Email + ")" })
                    .ToListAsync();
                return View(model);
            }

            // Sinh mã thẻ <= 20 ký tự (đúng unique index The_code)
            // Dạng: TH + yyMMdd + 6 ký tự ngẫu nhiên => tối đa 2+6+6=14 ký tự
            string code;
            do
            {
                code = $"TH{DateTime.UtcNow:yyMMdd}{Guid.NewGuid().ToString("N")[..6]}";
            } while (await _db.Thes.AnyAsync(t => t.TheCode == code));

            model.TheCode = code;

            _db.Thes.Add(model);
            await _db.SaveChangesAsync();
            TempData["Ok"] = "Cấp thẻ thành công.";
            return RedirectToAction(nameof(Index));
        }

        // Khóa thẻ
        [HttpPost]
        public async Task<IActionResult> Block(int id)
        {
            var card = await _db.Thes.FindAsync(id);
            if (card == null) return NotFound();
            card.TrangThai = "blocked";
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Gia hạn +1 năm
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

        // AJAX: lấy hạn mức theo user (dựa vào role_id của user)
        [HttpGet]
        public async Task<IActionResult> LimitOfUser(int userId)
        {
            var user = await _db.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return NotFound();

            var hm = await _db.HanMucs.Include(x => x.Role).FirstOrDefaultAsync(x => x.RoleId == user.RoleId);
            if (hm == null) return Json(new { ok = false });

            return Json(new
            {
                ok = true,
                role = user.Role.Name,
                maxBooks = hm.MaxBooks,
                maxDays = hm.MaxDays,
                maxRenewals = hm.MaxRenewals,
                maxFines = hm.MaxFines
            });
        }
    }
}
