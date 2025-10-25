using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLThuvien.Data;
using QLThuvien.Models;

namespace QLThuvien.Controllers
{
    [ApiController]
    public class ThongKeController : ControllerBase
    {
        private readonly ThuVienDbContext _context;

        public ThongKeController(ThuVienDbContext context)
        {
            _context = context;
        }

        // 📘 1. Sách mượn nhiều nhất → /topsach
        [HttpGet]
        [Route("topsach")]
        public async Task<IActionResult> GetSachMuonNhieu()
        {
            var result = await _context.MuonTra
                .Include(mt => mt.BanTheSach)
                    .ThenInclude(bts => bts.DanhMucSach)
                .GroupBy(mt => mt.BanTheSach.DanhMucSachId)
                .Select(g => new
                {
                    DanhMucSachId = g.Key,
                    TenSach = g.First().BanTheSach.DanhMucSach.TenSach,
                    SoLanMuon = g.Count()
                })
                .OrderByDescending(x => x.SoLanMuon)
                .Take(10)
                .ToListAsync();

            return Ok(result);
        }

        // 🏢 2. Phòng được đặt nhiều nhất → /topphong
        [HttpGet]
        [Route("topphong")]
        public async Task<IActionResult> GetPhongBanNhieu()
        {
            var result = await _context.DatPhong
                .Include(dp => dp.Phong)
                .GroupBy(dp => dp.PhongId)
                .Select(g => new
                {
                    PhongId = g.Key,
                    TenPhong = g.First().Phong.TenPhong,
                    SoLanDat = g.Count()
                })
                .OrderByDescending(x => x.SoLanDat)
                .Take(10)
                .ToListAsync();

            return Ok(result);
        }

        // 📅 3. Lượt mượn sách theo tháng → /topsachthang
        [HttpGet]
        [Route("topsachthang")]
        public async Task<IActionResult> GetLuotMuonTheoThang()
        {
            var result = await _context.MuonTra
                .GroupBy(mt => new { mt.NgayMuon.Year, mt.NgayMuon.Month })
                .Select(g => new
                {
                    Nam = g.Key.Year,
                    Thang = g.Key.Month,
                    TongLuotMuon = g.Count()
                })
                .OrderByDescending(x => x.Nam).ThenByDescending(x => x.Thang)
                .ToListAsync();

            return Ok(result);
        }

        // 📅 4. Lượt đặt phòng theo tháng → /topphongthang
        [HttpGet]
        [Route("topphongthang")]
        public async Task<IActionResult> GetLuotDatPhongTheoThang()
        {
            var result = await _context.DatPhong
                .GroupBy(dp => new { dp.GioBatDau.Year, dp.GioBatDau.Month })
                .Select(g => new
                {
                    Nam = g.Key.Year,
                    Thang = g.Key.Month,
                    TongLuotDat = g.Count()
                })
                .OrderByDescending(x => x.Nam).ThenByDescending(x => x.Thang)
                .ToListAsync();

            return Ok(result);
        }
    }
}