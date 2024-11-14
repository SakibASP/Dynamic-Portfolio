using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.Models;
using Portfolio.Web.Common;
using Portfolio.Web.Data;

namespace Portfolio.Web.Controllers
{
    [Authorize]
    public class EXPERIENCEController(ApplicationDbContext context) : BaseController
    {
        private readonly ApplicationDbContext _context = context;

        // GET: EXPERIENCE
        public async Task<IActionResult> Index()
        {
              return _context.EXPERIENCE != null ? 
                          View(await _context.EXPERIENCE.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.EXPERIENCE'  is null.");
        }

        // GET: EXPERIENCE/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.EXPERIENCE == null)
            {
                return NotFound();
            }

            var eXPERIENCE = await _context.EXPERIENCE
                .FirstOrDefaultAsync(m => m.AUTO_ID == id);
            if (eXPERIENCE == null)
            {
                return NotFound();
            }

            return View(eXPERIENCE);
        }

        // GET: EXPERIENCE/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EXPERIENCE/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AUTO_ID,DESIGNATION,FROM_DATE,TO_DATE,INSTITUTE,DES_1,DES_2,DES_3,DES_4")] EXPERIENCE eXPERIENCE)
        {
            if (ModelState.IsValid)
            {
                if (String.IsNullOrEmpty(eXPERIENCE.TO_DATE))
                    eXPERIENCE.TO_DATE = "Present";

                _context.Add(eXPERIENCE);
                await _context.SaveChangesAsync();
                //HttpContext.Session.Remove(Constant.myExperience);

                return RedirectToAction(nameof(Index));
            }
            return View(eXPERIENCE);
        }

        // GET: EXPERIENCE/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.EXPERIENCE == null)
            {
                return NotFound();
            }

            var eXPERIENCE = await _context.EXPERIENCE.FindAsync(id);
            if (eXPERIENCE == null)
            {
                return NotFound();
            }
            return View(eXPERIENCE);
        }

        // POST: EXPERIENCE/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AUTO_ID,DESIGNATION,FROM_DATE,TO_DATE,INSTITUTE,DES_1,DES_2,DES_3,DES_4")] EXPERIENCE eXPERIENCE)
        {
            if (id != eXPERIENCE.AUTO_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (String.IsNullOrEmpty(eXPERIENCE.TO_DATE))
                        eXPERIENCE.TO_DATE = "Present";

                    _context.Update(eXPERIENCE);
                    await _context.SaveChangesAsync();
                    //HttpContext.Session.Remove(Constant.myExperience);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EXPERIENCEExists(eXPERIENCE.AUTO_ID))
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
            return View(eXPERIENCE);
        }

        // GET: EXPERIENCE/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.EXPERIENCE == null)
            {
                return NotFound();
            }

            var eXPERIENCE = await _context.EXPERIENCE
                .FirstOrDefaultAsync(m => m.AUTO_ID == id);
            if (eXPERIENCE == null)
            {
                return NotFound();
            }

            return View(eXPERIENCE);
        }

        // POST: EXPERIENCE/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.EXPERIENCE == null)
            {
                return Problem("Entity set 'ApplicationDbContext.EXPERIENCE'  is null.");
            }
            var eXPERIENCE = await _context.EXPERIENCE.FindAsync(id);
            if (eXPERIENCE != null)
            {
                _context.EXPERIENCE.Remove(eXPERIENCE);
            }
            
            await _context.SaveChangesAsync();
            //HttpContext.Session.Remove(Constant.myExperience);

            return RedirectToAction(nameof(Index));
        }

        private bool EXPERIENCEExists(int id)
        {
          return (_context.EXPERIENCE?.Any(e => e.AUTO_ID == id)).GetValueOrDefault();
        }
    }
}
