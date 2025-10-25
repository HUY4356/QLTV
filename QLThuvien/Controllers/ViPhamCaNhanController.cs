using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLThuvien.Data;
using QLThuvien.Models;
using System.Threading.Tasks;

namespace QLThuvien.Controllers
{
    [Authorize]
    public class ViPhamCaNhanController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ViPhamCaNhanController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user?.Id == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var viPham = await _context.ViPhamUsers
                .Include(vp => vp.User)
                .FirstOrDefaultAsync(vp => vp.UserId == user.Id);

            return View(viPham);
        }
    }
}