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
    public class DatTruocSachesController : Controller
    {
        private readonly QL_ThuVienContext _context;

        public DatTruocSachesController(QL_ThuVienContext context)
        {
            _context = context;
        }

        // GET: DatTruocSaches
        public async Task<IActionResult> Index()
        {
            var qL_ThuVienContext = _context.DatTruocSach.Include(d => d.DanhMucSach).Include(d => d.User);
            return View(await qL_ThuVienContext.ToListAsync());
        }

        // GET: DatTruocSaches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var datTruocSach = await _context.DatTruocSach
                .Include(d => d.DanhMucSach)
                .Include(d => d.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (datTruocSach == null)
            {
                return NotFound();
            }

            return View(datTruocSach);
        }

        // GET: DatTruocSaches/Create
        public IActionResult Create()
        {
            ViewData["DanhMucSachId"] = new SelectList(_context.DanhMucSach, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Id");
            return View();
        }

        // POST: DatTruocSaches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DanhMucSachId,UserId,NgayDat,TrangThai")] DatTruocSach datTruocSach)
        {
            if (ModelState.IsValid)
            {
                _context.Add(datTruocSach);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DanhMucSachId"] = new SelectList(_context.DanhMucSach, "Id", "Id", datTruocSach.DanhMucSachId);
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Id", datTruocSach.UserId);
            return View(datTruocSach);
        }

        // GET: DatTruocSaches/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var datTruocSach = await _context.DatTruocSach.FindAsync(id);
            if (datTruocSach == null)
            {
                return NotFound();
            }
            ViewData["DanhMucSachId"] = new SelectList(_context.DanhMucSach, "Id", "Id", datTruocSach.DanhMucSachId);
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Id", datTruocSach.UserId);
            return View(datTruocSach);
        }

        // POST: DatTruocSaches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DanhMucSachId,UserId,NgayDat,TrangThai")] DatTruocSach datTruocSach)
        {
            if (id != datTruocSach.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(datTruocSach);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DatTruocSachExists(datTruocSach.Id))
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
            ViewData["DanhMucSachId"] = new SelectList(_context.DanhMucSach, "Id", "Id", datTruocSach.DanhMucSachId);
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Id", datTruocSach.UserId);
            return View(datTruocSach);
        }

        // GET: DatTruocSaches/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var datTruocSach = await _context.DatTruocSach
                .Include(d => d.DanhMucSach)
                .Include(d => d.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (datTruocSach == null)
            {
                return NotFound();
            }

            return View(datTruocSach);
        }

        // POST: DatTruocSaches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var datTruocSach = await _context.DatTruocSach.FindAsync(id);
            if (datTruocSach != null)
            {
                _context.DatTruocSach.Remove(datTruocSach);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DatTruocSachExists(int id)
        {
            return _context.DatTruocSach.Any(e => e.Id == id);
        }
    }
}
