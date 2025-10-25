using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLThuvien.Data;
using QLThuvien.Models;
using QLThuvien.Models.Dto;
using QLThuvien.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace QLThuvien.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class PhongsController : Controller
    {
        private readonly ThuVienDbContext _context;

        public PhongsController(ThuVienDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(RoomStatus? trangThai, DateTime? tuNgay, DateTime? denNgay)
        {
            var danhSachPhong = await _context.Phongs
                .Where(p => !trangThai.HasValue || p.TrangThai == trangThai)
                .ToListAsync();

            var lichDatPhong = await _context.DatPhongs
                .Where(dp => dp.GioKetThuc > DateTime.UtcNow &&
                             (!tuNgay.HasValue || dp.GioBatDau >= tuNgay.Value) &&
                             (!denNgay.HasValue || dp.GioKetThuc <= denNgay.Value))
                .ToListAsync();

            var lichTheoPhong = lichDatPhong
                .GroupBy(dp => dp.PhongId)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(dp => new DatPhongDto(dp)).ToList()
                );

            var viewModel = new PhongViewModel
            {
                DanhSachPhong = danhSachPhong,
                LichDatPhong = lichTheoPhong,
                CurrentUserId = null
            };

            return View(viewModel);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Phong model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.CreatedAt = DateTime.UtcNow;
            _context.Phongs.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var phong = await _context.Phongs.FindAsync(id);
            if (phong == null)
            {
                return NotFound();
            }

            return View(phong);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Phong model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _context.Phongs.Update(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var phong = await _context.Phongs.FindAsync(id);
            if (phong == null)
            {
                return NotFound();
            }

            _context.Phongs.Remove(phong);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}