using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLThuvien.Models;

namespace QLThuvien.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,ThuThu")]
    public class HanMucsController : Controller
    {
        private readonly ThuVienDbContext _db;
        public HanMucsController(ThuVienDbContext db) => _db = db;

        public async Task<IActionResult> Index()
            => View(await _db.HanMucs.Include(h => h.Role).ToListAsync());

        public async Task<IActionResult> Create()
        {
            ViewBag.Roles = await _db.Roles.ToListAsync();
            return View(new HanMuc { MaxBooks = 3, MaxDays = 14, MaxRenewals = 1, MaxFines = 0 });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HanMuc m)
        {
            if (!await _db.Roles.AnyAsync(r => r.Id == m.RoleId))
                ModelState.AddModelError(nameof(m.RoleId), "Vai trò không tồn tại.");

            //if (await _db.HanMucs.AnyAsync(x => x.RoleId == m.RoleId))
            //    ModelState.AddModelError(string.Empty, "Vai trò này đã có hạn mức.");

            if (!ModelState.IsValid)
            {
                ViewBag.Roles = await _db.Roles.ToListAsync();
                return View(m);
            }
            _db.HanMucs.Add(m);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var m = await _db.HanMucs.FindAsync(id);
            if (m == null) return NotFound();
            ViewBag.Roles = await _db.Roles.ToListAsync();
            return View(m);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(HanMuc m)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Roles = await _db.Roles.ToListAsync();
                return View(m);
            }
            _db.Update(m);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
