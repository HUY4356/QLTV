using Microsoft.AspNetCore.Mvc;
using QLThuvien.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace QLThuvien.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DatPhongController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DatPhongController(ApplicationDbContext context)
        {
            _context = context;
        }

        // API đặt phòng
        [HttpPost("dat-phong")]
        public IActionResult DatPhong([FromBody] DatPhong model)
        {
            // Kiểm tra phòng đã được đặt trong khoảng thời gian này chưa
            var trungLich = _context.DatPhongs.Any(dp =>
                dp.PhongId == model.PhongId &&
                dp.TrangThai != "Đã hủy" &&
                (
                    (model.GioBatDau >= dp.GioBatDau && model.GioBatDau < dp.GioKetThuc) ||
                    (model.GioKetThuc > dp.GioBatDau && model.GioKetThuc <= dp.GioKetThuc) ||
                    (model.GioBatDau <= dp.GioBatDau && model.GioKetThuc >= dp.GioKetThuc)
                )
            );

            if (trungLich)
            {
                return BadRequest("Phòng đã được đặt trong khoảng thời gian này.");
            }

            model.NgayDat = DateTime.Now;
            model.TrangThai = "Đã đặt";

            _context.DatPhongs.Add(model);
            _context.SaveChanges();

            return Ok(model);
        }

        // API lấy danh sách đặt phòng theo phòng hoặc user
        [HttpGet("list")]
        public IActionResult GetList(int? phongId = null, int? userId = null)
        {
            var query = _context.DatPhongs
                .Include(dp => dp.Phong)
                .Include(dp => dp.User)
                .AsQueryable();

            if (phongId.HasValue)
                query = query.Where(dp => dp.PhongId == phongId.Value);

            if (userId.HasValue)
                query = query.Where(dp => dp.UserId == userId.Value);

            return Ok(query.ToList());
        }

        // API hủy đặt phòng
        [HttpPost("huy")]
        public IActionResult HuyDatPhong(int id)
        {
            var datPhong = _context.DatPhongs.Find(id);
            if (datPhong == null)
                return NotFound();

            datPhong.TrangThai = "Đã hủy";
            _context.SaveChanges();

            return Ok(datPhong);
        }
    }
}