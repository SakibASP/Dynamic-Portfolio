using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Portfolio.Models;
using Portfolio.Web.Common;
using Portfolio.Web.Data;

namespace Portfolio.Web.Controllers
{
    public class DESCRIPTIONsController(ApplicationDbContext context) : BaseController
    {
        private readonly ApplicationDbContext _context = context;

        // GET: DESCRIPTIONs
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.DESCRIPTION.Include(d => d.DESCRIPTION_TYPE_).Include(p=>p.PROJECT_);
            return View(await applicationDbContext.OrderBy(x=>x.PROJECT_ID).ThenBy(x=>x.SORT_ORDER).ToListAsync());
        }

        // GET: DESCRIPTIONs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dESCRIPTION = await _context.DESCRIPTION
                .Include(d => d.DESCRIPTION_TYPE_)
                .FirstOrDefaultAsync(m => m.AUTO_ID == id);
            if (dESCRIPTION == null)
            {
                return NotFound();
            }

            return View(dESCRIPTION);
        }

        // GET: DESCRIPTIONs/Create
        public IActionResult Create()
        {
            ViewData["TYPE_ID"] = new SelectList(_context.DESCRIPTION_TYPE, "AUTO_ID", "TYPE");
            ViewData["PROJECT_ID"] = new SelectList(_context.PROJECTS, "AUTO_ID", "PROJECT_NAME");
            ViewData["EXPERIENCE_ID"] = new SelectList(_context.EXPERIENCE, "AUTO_ID", "INSTITUTE");
            return View();
        }

        // POST: DESCRIPTIONs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DESCRIPTION dESCRIPTION)
        {
            if (ModelState.IsValid)
            {
                dESCRIPTION.CREATED_BY = User.Identity!.Name;
                dESCRIPTION.CREATED_DATE = DateTime.Now;
                _context.Add(dESCRIPTION);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TYPE_ID"] = new SelectList(_context.DESCRIPTION_TYPE, "AUTO_ID", "TYPE", dESCRIPTION.TYPE_ID);
            ViewData["PROJECT_ID"] = new SelectList(_context.PROJECTS, "AUTO_ID", "PROJECT_NAME", dESCRIPTION.PROJECT_ID);
            ViewData["EXPERIENCE_ID"] = new SelectList(_context.EXPERIENCE, "AUTO_ID", "INSTITUTE", dESCRIPTION.EXPERIENCE_ID);
            return View(dESCRIPTION);
        }

        // GET: DESCRIPTIONs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dESCRIPTION = await _context.DESCRIPTION.FindAsync(id);
            if (dESCRIPTION == null)
            {
                return NotFound();
            }
            ViewData["TYPE_ID"] = new SelectList(_context.DESCRIPTION_TYPE, "AUTO_ID", "TYPE", dESCRIPTION.TYPE_ID);
            ViewData["PROJECT_ID"] = new SelectList(_context.PROJECTS, "AUTO_ID", "PROJECT_NAME", dESCRIPTION.PROJECT_ID);
            ViewData["EXPERIENCE_ID"] = new SelectList(_context.EXPERIENCE, "AUTO_ID", "INSTITUTE", dESCRIPTION.EXPERIENCE_ID);
            return View(dESCRIPTION);
        }

        // POST: DESCRIPTIONs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DESCRIPTION dESCRIPTION)
        {
            if (id != dESCRIPTION.AUTO_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    dESCRIPTION.MODIFIED_BY = User.Identity!.Name;
                    dESCRIPTION.MODIFIED_DATE = DateTime.Now;
                    _context.Update(dESCRIPTION);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DESCRIPTIONExists(dESCRIPTION.AUTO_ID))
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
            ViewData["TYPE_ID"] = new SelectList(_context.DESCRIPTION_TYPE, "AUTO_ID", "TYPE", dESCRIPTION.TYPE_ID);
            ViewData["PROJECT_ID"] = new SelectList(_context.PROJECTS, "AUTO_ID", "PROJECT_NAME", dESCRIPTION.PROJECT_ID);
            ViewData["EXPERIENCE_ID"] = new SelectList(_context.EXPERIENCE, "AUTO_ID", "INSTITUTE", dESCRIPTION.EXPERIENCE_ID);
            return View(dESCRIPTION);
        }

        // GET: DESCRIPTIONs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dESCRIPTION = await _context.DESCRIPTION
                .Include(d => d.DESCRIPTION_TYPE_)
                .FirstOrDefaultAsync(m => m.AUTO_ID == id);
            if (dESCRIPTION == null)
            {
                return NotFound();
            }

            return View(dESCRIPTION);
        }

        // POST: DESCRIPTIONs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dESCRIPTION = await _context.DESCRIPTION.FindAsync(id);
            if (dESCRIPTION != null)
            {
                _context.DESCRIPTION.Remove(dESCRIPTION);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DESCRIPTIONExists(int id)
        {
            return _context.DESCRIPTION.Any(e => e.AUTO_ID == id);
        }
    }
}
