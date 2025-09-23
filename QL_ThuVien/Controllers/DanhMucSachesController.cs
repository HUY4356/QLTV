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
    public class DanhMucSachesController : Controller
    {
        private readonly QL_ThuVienContext _context;

        public DanhMucSachesController(QL_ThuVienContext context)
        {
            _context = context;
        }

        // GET: DanhMucSaches
        public async Task<IActionResult> Index()
        {
            var qL_ThuVienContext = _context.DanhMucSach.Include(d => d.NhomSach);
            return View(await qL_ThuVienContext.ToListAsync());
        }

        // GET: DanhMucSaches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var danhMucSach = await _context.DanhMucSach
                .Include(d => d.NhomSach)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (danhMucSach == null)
            {
                return NotFound();
            }

            return View(danhMucSach);
        }

        // GET: DanhMucSaches/Create
        public IActionResult Create()
        {
            ViewData["NhomSachId"] = new SelectList(_context.NhomSach, "Id", "Id");
            return View();
        }

        // POST: DanhMucSaches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NhomSachId,MaSach,TenSach,TacGia,DonGia,Slton,ViTriKe")] DanhMucSach danhMucSach)
        {
            if (ModelState.IsValid)
            {
                _context.Add(danhMucSach);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["NhomSachId"] = new SelectList(_context.NhomSach, "Id", "Id", danhMucSach.NhomSachId);
            return View(danhMucSach);
        }

        // GET: DanhMucSaches/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var danhMucSach = await _context.DanhMucSach.FindAsync(id);
            if (danhMucSach == null)
            {
                return NotFound();
            }
            ViewData["NhomSachId"] = new SelectList(_context.NhomSach, "Id", "Id", danhMucSach.NhomSachId);
            return View(danhMucSach);
        }

        // POST: DanhMucSaches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NhomSachId,MaSach,TenSach,TacGia,DonGia,Slton,ViTriKe")] DanhMucSach danhMucSach)
        {
            if (id != danhMucSach.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(danhMucSach);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DanhMucSachExists(danhMucSach.Id))
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
            ViewData["NhomSachId"] = new SelectList(_context.NhomSach, "Id", "Id", danhMucSach.NhomSachId);
            return View(danhMucSach);
        }

        // GET: DanhMucSaches/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var danhMucSach = await _context.DanhMucSach
                .Include(d => d.NhomSach)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (danhMucSach == null)
            {
                return NotFound();
            }

            return View(danhMucSach);
        }

        // POST: DanhMucSaches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var danhMucSach = await _context.DanhMucSach.FindAsync(id);
            if (danhMucSach != null)
            {
                _context.DanhMucSach.Remove(danhMucSach);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DanhMucSachExists(int id)
        {
            return _context.DanhMucSach.Any(e => e.Id == id);
        }
    }
}
