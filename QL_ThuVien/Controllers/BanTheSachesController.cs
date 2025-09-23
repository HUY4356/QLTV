using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QL_ThuVien.Data;
using QL_ThuVien.Models;

namespace QL_ThuVien.Controllers
{
    public class BanTheSachesController : Controller
    {
        private readonly QL_ThuVienContext _context;

        public BanTheSachesController(QL_ThuVienContext context)
        {
            _context = context;
        }

        // GET: BanTheSaches
        public async Task<IActionResult> Index()
        {
            var qL_ThuVienContext = _context.BanTheSach.Include(b => b.DanhMucSach);
            return View(await qL_ThuVienContext.ToListAsync());
        }

        // GET: BanTheSaches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var banTheSach = await _context.BanTheSach
                .Include(b => b.DanhMucSach)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (banTheSach == null)
            {
                return NotFound();
            }

            return View(banTheSach);
        }

        // GET: BanTheSaches/Create
        public IActionResult Create()
        {
            ViewData["DanhMucSachId"] = new SelectList(_context.DanhMucSach, "Id", "Id");
            return View();
        }

        // POST: BanTheSaches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DanhMucSachId,MaVach,TinhTrang")] BanTheSach banTheSach)
        {
            if (ModelState.IsValid)
            {
                _context.Add(banTheSach);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DanhMucSachId"] = new SelectList(_context.DanhMucSach, "Id", "Id", banTheSach.DanhMucSachId);
            return View(banTheSach);
        }

        // GET: BanTheSaches/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var banTheSach = await _context.BanTheSach.FindAsync(id);
            if (banTheSach == null)
            {
                return NotFound();
            }
            ViewData["DanhMucSachId"] = new SelectList(_context.DanhMucSach, "Id", "Id", banTheSach.DanhMucSachId);
            return View(banTheSach);
        }

        // POST: BanTheSaches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DanhMucSachId,MaVach,TinhTrang")] BanTheSach banTheSach)
        {
            if (id != banTheSach.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(banTheSach);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BanTheSachExists(banTheSach.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DanhMucSachId"] = new SelectList(_context.DanhMucSach, "Id", "Id", banTheSach.DanhMucSachId);
            return View(banTheSach);
        }

        // GET: BanTheSaches/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var banTheSach = await _context.BanTheSach
                .Include(b => b.DanhMucSach)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (banTheSach == null)
            {
                return NotFound();
            }

            return View(banTheSach);
        }

        // POST: BanTheSaches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var banTheSach = await _context.BanTheSach.FindAsync(id);
            if (banTheSach != null)
            {
                _context.BanTheSach.Remove(banTheSach);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BanTheSachExists(int id)
        {
            return _context.BanTheSach.Any(e => e.Id == id);
        }
    }
}
