using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.Models;
using Portfolio.Web.Common;
using Portfolio.Web.Data;

namespace Portfolio.Web.Controllers
{
    [Authorize]
    public class EDUCATIONController(ApplicationDbContext context) : BaseController
    {
        private readonly ApplicationDbContext _context = context;

        // GET: EDUCATION
        public async Task<IActionResult> Index()
        {
              return _context.EDUCATION != null ? 
                          View(await _context.EDUCATION.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.EDUCATION'  is null.");
        }

        // GET: EDUCATION/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.EDUCATION == null)
            {
                return NotFound();
            }

            var eDUCATION = await _context.EDUCATION
                .FirstOrDefaultAsync(m => m.AUTO_ID == id);
            if (eDUCATION == null)
            {
                return NotFound();
            }

            return View(eDUCATION);
        }

        // GET: EDUCATION/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EDUCATION/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AUTO_ID,COURSE,FROM_DATE,TO_DATE,INSTITUTE,DESCRIPTION")] EDUCATION eDUCATION)
        {
            if (ModelState.IsValid)
            {
                if (String.IsNullOrEmpty(eDUCATION.TO_DATE))
                    eDUCATION.TO_DATE = "Present";

                _context.Add(eDUCATION);
                await _context.SaveChangesAsync();
                //HttpContext.Session.Remove(Constant.myEducation);

                return RedirectToAction(nameof(Index));
            }
            return View(eDUCATION);
        }

        // GET: EDUCATION/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.EDUCATION == null)
            {
                return NotFound();
            }

            var eDUCATION = await _context.EDUCATION.FindAsync(id);
            if (eDUCATION == null)
            {
                return NotFound();
            }
            return View(eDUCATION);
        }

        // POST: EDUCATION/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AUTO_ID,COURSE,FROM_DATE,TO_DATE,INSTITUTE,DESCRIPTION")] EDUCATION eDUCATION)
        {
            if (id != eDUCATION.AUTO_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (String.IsNullOrEmpty(eDUCATION.TO_DATE))
                        eDUCATION.TO_DATE = "Present";

                    _context.Update(eDUCATION);
                    await _context.SaveChangesAsync();
                    //HttpContext.Session.Remove(Constant.myEducation);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EDUCATIONExists(eDUCATION.AUTO_ID))
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
            return View(eDUCATION);
        }

        // GET: EDUCATION/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.EDUCATION == null)
            {
                return NotFound();
            }

            var eDUCATION = await _context.EDUCATION
                .FirstOrDefaultAsync(m => m.AUTO_ID == id);
            if (eDUCATION == null)
            {
                return NotFound();
            }

            return View(eDUCATION);
        }

        // POST: EDUCATION/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.EDUCATION == null)
            {
                return Problem("Entity set 'ApplicationDbContext.EDUCATION'  is null.");
            }
            var eDUCATION = await _context.EDUCATION.FindAsync(id);
            if (eDUCATION != null)
            {
                _context.EDUCATION.Remove(eDUCATION);
            }
            
            await _context.SaveChangesAsync();
            //HttpContext.Session.Remove(Constant.myEducation);

            return RedirectToAction(nameof(Index));
        }

        private bool EDUCATIONExists(int id)
        {
          return (_context.EDUCATION?.Any(e => e.AUTO_ID == id)).GetValueOrDefault();
        }
    }
}
