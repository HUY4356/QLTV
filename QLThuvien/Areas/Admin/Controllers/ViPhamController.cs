using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLThuvien.Data;
using QLThuvien.Models;
using QLThuvien.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace QLThuvien.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,ThuThu")]
    public class ViPhamController : Controller
    {
        private readonly ThuVienDbContext _context;

        public ViPhamController(ThuVienDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchTerm)
        {
            var query = _context.ViPhamUsers.Include(vp => vp.User).AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                string lower = searchTerm.ToLower();
                query = query.Where(vp =>
                    (vp.User.Fullname != null && vp.User.Fullname.ToLower().Contains(lower)) ||
                    (vp.User.Email != null && vp.User.Email.ToLower().Contains(lower)));
            }

            var model = await query
                .Where(vp => vp.CountNoShow > 0 || vp.CountTreHan > 0)
                .OrderByDescending(vp => vp.CountNoShow + vp.CountTreHan)
                .ThenByDescending(vp => vp.LastUpdated)
                .ToListAsync();

            return View(model);
        }
    }
}