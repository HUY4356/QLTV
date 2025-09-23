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
    public class NhomSachesController : Controller
    {
        private readonly QL_ThuVienContext _context;

        public NhomSachesController(QL_ThuVienContext context)
        {
            _context = context;
        }

        // GET: NhomSaches
        public async Task<IActionResult> Index()
        {
            return View(await _context.NhomSach.ToListAsync());
        }

        // GET: NhomSaches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nhomSach = await _context.NhomSach
                .FirstOrDefaultAsync(m => m.Id == id);
            if (nhomSach == null)
            {
                return NotFound();
            }

            return View(nhomSach);
        }

        // GET: NhomSaches/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: NhomSaches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MaNhom,TenNhom")] NhomSach nhomSach)
        {
            if (ModelState.IsValid)
            {
                _context.Add(nhomSach);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(nhomSach);
        }

        // GET: NhomSaches/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nhomSach = await _context.NhomSach.FindAsync(id);
            if (nhomSach == null)
            {
                return NotFound();
            }
            return View(nhomSach);
        }

        // POST: NhomSaches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MaNhom,TenNhom")] NhomSach nhomSach)
        {
            if (id != nhomSach.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nhomSach);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NhomSachExists(nhomSach.Id))
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
            return View(nhomSach);
        }

        // GET: NhomSaches/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nhomSach = await _context.NhomSach
                .FirstOrDefaultAsync(m => m.Id == id);
            if (nhomSach == null)
            {
                return NotFound();
            }

            return View(nhomSach);
        }

        // POST: NhomSaches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var nhomSach = await _context.NhomSach.FindAsync(id);
            if (nhomSach != null)
            {
                _context.NhomSach.Remove(nhomSach);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NhomSachExists(int id)
        {
            return _context.NhomSach.Any(e => e.Id == id);
        }
    }
}
