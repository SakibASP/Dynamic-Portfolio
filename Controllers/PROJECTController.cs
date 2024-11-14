using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SAKIB_PORTFOLIO.Common;
using SAKIB_PORTFOLIO.Data;
using SAKIB_PORTFOLIO.Models;

namespace SAKIB_PORTFOLIO.Controllers
{
    public class PROJECTSController(ApplicationDbContext context) : BaseController
    {
        private readonly ApplicationDbContext _context = context;

        // GET: SERVICES
        public async Task<IActionResult> Index()
        {
              return _context.PROJECTS != null ? 
                          View(await _context.PROJECTS.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.PROJECTS'  is null.");
        }

        // GET: SERVICES/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PROJECTS == null)
            {
                return NotFound();
            }

            var PROJECTS = await _context.PROJECTS
                .FirstOrDefaultAsync(m => m.AUTO_ID == id);
            if (PROJECTS == null)
            {
                return NotFound();
            }
            ViewData["DESCRIPTIONs"] = await _context.DESCRIPTION.Where(x => x.PROJECT_ID == id).OrderBy(x=>x.SORT_ORDER).AsNoTracking().ToListAsync();

            return View(PROJECTS);
        }

        [Authorize]
        // GET: PROJECTS/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PROJECTS/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PROJECTS pROJECTS)
        {
            if (ModelState.IsValid)
            {
                var imgFile = Request.Form.Files.FirstOrDefault();
                if (imgFile != null)
                {
                    pROJECTS.LOGO = await Utility.Getimage(pROJECTS.LOGO, Request.Form.Files);
                }

                _context.Add(pROJECTS);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(pROJECTS);
        }

        // GET: SERVICES/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PROJECTS == null)
            {
                return NotFound();
            }

            var PROJECTS = await _context.PROJECTS.FindAsync(id);
            if (PROJECTS == null)
            {
                return NotFound();
            }
            return View(PROJECTS);
        }

        // POST: PROJECTS/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PROJECTS pROJECTS)
        {
            if (id != pROJECTS.AUTO_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var imgFile = Request.Form.Files.FirstOrDefault();
                    if (imgFile != null)
                    {
                        pROJECTS.LOGO = await Utility.Getimage(pROJECTS.LOGO, Request.Form.Files);
                    }

                    _context.Update(pROJECTS);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PROJECTSExists(pROJECTS.AUTO_ID))
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
            return View(pROJECTS);
        }

        // GET: PROJECTS/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PROJECTS == null)
            {
                return NotFound();
            }

            var PROJECTS = await _context.PROJECTS
                .FirstOrDefaultAsync(m => m.AUTO_ID == id);
            if (PROJECTS == null)
            {
                return NotFound();
            }

            return View(PROJECTS);
        }

        // POST: PROJECTS/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PROJECTS == null)
            {
                return Problem("Entity set 'ApplicationDbContext.PROJECTS'  is null.");
            }
            var PROJECTS = await _context.PROJECTS.FindAsync(id);
            if (PROJECTS != null)
            {
                _context.PROJECTS.Remove(PROJECTS);
            }
            
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool PROJECTSExists(int id)
        {
          return (_context.PROJECTS?.Any(e => e.AUTO_ID == id)).GetValueOrDefault();
        }
    }
}
