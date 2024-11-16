using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.Models;
using Portfolio.Web.Common;
using Portfolio.Web.Data;

namespace Portfolio.Web.Controllers
{
    [Authorize]
    public class MY_SKILLController(ApplicationDbContext context) : BaseController
    {
        private readonly ApplicationDbContext _context = context;

        // GET: MY_SKILL
        public async Task<IActionResult> Index()
        {
              return _context.MY_SKILLS != null ? 
                          View(await _context.MY_SKILLS.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.MY_SKILLS'  is null.");
        }

        // GET: MY_SKILL/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.MY_SKILLS == null)
            {
                return NotFound();
            }

            var mY_SKILLS = await _context.MY_SKILLS
                .FirstOrDefaultAsync(m => m.AUTO_ID == id);
            if (mY_SKILLS == null)
            {
                return NotFound();
            }

            return View(mY_SKILLS);
        }

        // GET: MY_SKILL/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MY_SKILL/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MY_SKILLS mY_SKILLS)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mY_SKILLS);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(mY_SKILLS);
        }

        // GET: MY_SKILL/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.MY_SKILLS == null)
            {
                return NotFound();
            }

            var mY_SKILLS = await _context.MY_SKILLS.FindAsync(id);
            if (mY_SKILLS == null)
            {
                return NotFound();
            }
            return View(mY_SKILLS);
        }

        // POST: MY_SKILL/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MY_SKILLS mY_SKILLS)
        {
            if (id != mY_SKILLS.AUTO_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mY_SKILLS);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MY_SKILLSExists(mY_SKILLS.AUTO_ID))
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
            return View(mY_SKILLS);
        }

        // GET: MY_SKILL/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.MY_SKILLS == null)
            {
                return NotFound();
            }

            var mY_SKILLS = await _context.MY_SKILLS
                .FirstOrDefaultAsync(m => m.AUTO_ID == id);
            if (mY_SKILLS == null)
            {
                return NotFound();
            }

            return View(mY_SKILLS);
        }

        // POST: MY_SKILL/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.MY_SKILLS == null)
            {
                return Problem("Entity set 'ApplicationDbContext.MY_SKILLS'  is null.");
            }
            var mY_SKILLS = await _context.MY_SKILLS.FindAsync(id);
            if (mY_SKILLS != null)
            {
                _context.MY_SKILLS.Remove(mY_SKILLS);
            }
            
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool MY_SKILLSExists(int id)
        {
          return (_context.MY_SKILLS?.Any(e => e.AUTO_ID == id)).GetValueOrDefault();
        }
    }
}
