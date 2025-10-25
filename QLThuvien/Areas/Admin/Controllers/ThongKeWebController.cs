using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLThuvien.Data;
using QLThuvien.Models;

namespace QLThuvien.Controllers
{
    public class ThongKeWebController : Controller
    {
        private readonly ThuVienDbContext _context;

        public ThongKeWebController(ThuVienDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var topSach = await _context.MuonTra
                .Include(mt => mt.BanTheSach)
                    .ThenInclude(bts => bts.DanhMucSach)
                .GroupBy(mt => mt.BanTheSach.DanhMucSachId)
                .Select(g => new
                {
                    TenSach = g.First().BanTheSach.DanhMucSach.TenSach,
                    SoLanMuon = g.Count()
                })
                .OrderByDescending(x => x.SoLanMuon)
                .Take(10)
                .ToListAsync();

            var topPhong = await _context.DatPhong
                .Include(dp => dp.Phong)
                .GroupBy(dp => dp.PhongId)
                .Select(g => new
                {
                    TenPhong = g.First().Phong.TenPhong,
                    SoLanDat = g.Count()
                })
                .OrderByDescending(x => x.SoLanDat)
                .Take(10)
                .ToListAsync();

            ViewBag.TopSach = topSach;
            ViewBag.TopPhong = topPhong;

            return View();
        }
    }
}