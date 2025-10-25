using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QLThuvien.Models;

namespace QLThuvien.Controllers
{
    public class DatPhongController : Controller
    {
        private readonly ThuVienDbContext _context;

        public DatPhongController(ThuVienDbContext context)
        {
            _context = context;
        }

        // GET: DatPhong
        public async Task<IActionResult> Index()
        {
            var datPhongs = await _context.DatPhongs
                .Include(d => d.User)
                .Include(d => d.Phong)
                .OrderByDescending(d => d.NgayDat)
                .ToListAsync();

            return View(datPhongs);
        }

        // GET: DatPhong/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Fullname");
            ViewData["PhongId"] = new SelectList(_context.Phongs.Where(p => p.TrangThai == "Trống"), "Id", "TenPhong");
            return View();
        }

        // POST: DatPhong/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,PhongId,NgayDat,GioBatDau,GioKetThuc")] DatPhong datPhong)
        {
            if (ModelState.IsValid)
            {
                datPhong.TrangThai = "Đã đặt";
                _context.Add(datPhong);

                var phong = await _context.Phongs.FindAsync(datPhong.PhongId);
                if (phong != null)
                {
                    phong.TrangThai = "Đã đặt";
                    _context.Update(phong);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Fullname", datPhong.UserId);
            ViewData["PhongId"] = new SelectList(_context.Phongs, "Id", "TenPhong", datPhong.PhongId);
            return View(datPhong);
        }

        // GET: DatPhong/TrangThai/5
        public async Task<IActionResult> TrangThai(int id)
        {
            var datPhong = await _context.DatPhongs
                .Include(d => d.Phong)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (datPhong == null)
                return NotFound();

            return Content($"Phòng: {datPhong.Phong.TenPhong} - Trạng thái: {datPhong.TrangThai}");
        }
    }
}