using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.Models;
using Portfolio.Utils;
using Portfolio.Web.Common;
using Portfolio.Web.Data;
using Serilog;

namespace Portfolio.Web.Controllers
{
    [Authorize]
    public class PROFILE_COVERController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment) : BaseController
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

        // GET: PROFILE_COVER
        public async Task<IActionResult> Index()
        {
            ViewData["rootPath"] = _webHostEnvironment.WebRootPath;
            return View(await _context.PROFILE_COVER.ToListAsync());
        }

        // GET: PROFILE_COVER/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PROFILE_COVER == null)
            {
                return NotFound();
            }

            var pROFILE_COVER = await _context.PROFILE_COVER
                .FirstOrDefaultAsync(m => m.AUTO_ID == id);
            if (pROFILE_COVER == null)
            {
                return NotFound();
            }

            return View(pROFILE_COVER);
        }

        // GET: PROFILE_COVER/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PROFILE_COVER/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PROFILE_COVER pROFILE_COVER)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var files = Request.Form.Files.FirstOrDefault();
                    if (files != null)
                    {
                        const string rootFolder = @"Images\Cover";
                        string? directoryPath = Path.Combine(_webHostEnvironment.WebRootPath, rootFolder);
                        // Check if the directory exists; if not, create it
                        if (!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);
                        }

                        //Time in seconds
                        string formattedDateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                        var fileName = formattedDateTime + "_" + files.FileName;
                        var uploadPath = Path.Combine(directoryPath, fileName);

                        //saving the file
                        await Utility.SaveFileAsync(uploadPath, files);
                        pROFILE_COVER.COVER_IMAGE = uploadPath;

                        _context.Add(pROFILE_COVER);
                        await _context.SaveChangesAsync();

                    }

                    return RedirectToAction(nameof(Index));
                }
                catch(Exception ex)
                {
                    TempData[Constant.Error] = Constant.ErrorMessage;
                    Log.Error(ex, $"I am from {ControllerContext.ActionDescriptor.ControllerName} {ControllerContext.ActionDescriptor.MethodInfo.Name}... {User.Identity?.Name}");

                }
            }
            return View(pROFILE_COVER);
        }

        // GET: PROFILE_COVER/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PROFILE_COVER == null)
            {
                return NotFound();
            }

            var pROFILE_COVER = await _context.PROFILE_COVER.FindAsync(id);
            if (pROFILE_COVER == null)
            {
                return NotFound();
            }
            ViewData["rootPath"] = _webHostEnvironment.WebRootPath;
            return View(pROFILE_COVER);
        }

        // POST: PROFILE_COVER/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PROFILE_COVER pROFILE_COVER)
        {
            if (id != pROFILE_COVER.AUTO_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var files = Request.Form.Files.FirstOrDefault();
                    if (files != null)
                    {
                        const string rootFolder = @"Images\Cover";
                        string? directoryPath = Path.Combine(_webHostEnvironment.WebRootPath, rootFolder);
                        // Check if the directory exists; if not, create it
                        if (!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);
                        }

                        //Time in seconds
                        string formattedDateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                        var fileName = formattedDateTime + "_" + files.FileName;
                        var uploadPath = Path.Combine(directoryPath, fileName);
                        //delete old picture
                        if (System.IO.File.Exists(pROFILE_COVER?.COVER_IMAGE))
                            System.IO.File.Delete(pROFILE_COVER.COVER_IMAGE);
                        //saving the file
                        await Utility.SaveFileAsync(uploadPath, files);
                        pROFILE_COVER.COVER_IMAGE = uploadPath;

                        _context.Update(pROFILE_COVER);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PROFILE_COVERExists(pROFILE_COVER.AUTO_ID))
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
            return View(pROFILE_COVER);
        }

        // GET: PROFILE_COVER/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null || _context.PROFILE_COVER == null)
        //    {
        //        return NotFound();
        //    }

        //    var pROFILE_COVER = await _context.PROFILE_COVER
        //        .FirstOrDefaultAsync(m => m.AUTO_ID == id);
        //    if (pROFILE_COVER == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(pROFILE_COVER);
        //}

        // POST: PROFILE_COVER/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    if (_context.PROFILE_COVER == null)
        //    {
        //        return Problem("Entity set 'ApplicationDbContext.PROFILE_COVER'  is null.");
        //    }
        //    var pROFILE_COVER = await _context.PROFILE_COVER.FindAsync(id);
        //    if (pROFILE_COVER != null)
        //    {
        //        _context.PROFILE_COVER.Remove(pROFILE_COVER);
        //    }
            
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool PROFILE_COVERExists(int id)
        {
          return _context.PROFILE_COVER.Any(e => e.AUTO_ID == id);
        }
    }
}
