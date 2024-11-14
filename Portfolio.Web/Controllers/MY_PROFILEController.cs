using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Portfolio.Models;
using Portfolio.Web.Common;
using Portfolio.Web.Data;
using Portfolio.Models;
using Portfolio.Utils;

namespace Portfolio.Web.Controllers
{
    public class MY_PROFILEController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment) : BaseController
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

        // GET: MY_PROFILE
        [Authorize]
        public async Task<IActionResult> Index()
        {
            ViewData["rootPath"] = _webHostEnvironment.WebRootPath;
            return _context.MY_PROFILE != null ? 
                          View(await _context.MY_PROFILE.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.MY_PROFILE'  is null.");
        }

        public async Task<IActionResult> About()
        {
            ViewData["rootPath"] = _webHostEnvironment.WebRootPath;
            ViewData["SKILLS"] = await _context.MY_SKILLS.AsNoTracking().ToListAsync();
            ViewData["PROFILES"] = await _context.MY_PROFILE.AsNoTracking().FirstOrDefaultAsync();
            return View();
        }

        public async Task<IActionResult> Resume()
        {
            ViewData["PROFILES"] = await _context.MY_PROFILE.AsNoTracking().FirstOrDefaultAsync();
            ViewData["EDUCATIONS"] = await _context.EDUCATION.AsNoTracking().ToListAsync(); 
            ViewData["EXPERIENCEs"] = await _context.EXPERIENCE.AsNoTracking().ToListAsync();
            ViewData["DESCRIPTIONs"] = await _context.DESCRIPTION.Include(x=>x.DESCRIPTION_TYPE_).AsNoTracking().ToListAsync();
            return View();
        }

        public async Task<IActionResult> Projects()
        {
            ViewData["PROFILES"] = await _context.MY_PROFILE.AsNoTracking().FirstOrDefaultAsync();
            ViewData["PROJECTS"] = await _context.PROJECTS.AsNoTracking().ToListAsync();
            return View();
        }

        public async Task<IActionResult> Contact()
        {
            ViewData["PROFILES"] = await _context.MY_PROFILE.AsNoTracking().FirstOrDefaultAsync();
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Contact(CONTACTS objContact)
        {
            try
            {
                //sending email if it is not null
                if (!string.IsNullOrEmpty(objContact.EMAIL))
                {
                    try
                    {
                        EmailSettings email = new();
                        SendEmail sendEmail = new(email);
                        const string subject = "Welcome";
                        const string htmlMessage = "Thanks for contacting with me. I will response as soon as possible";
                        await sendEmail.SendEmailAsync(objContact.EMAIL, subject, htmlMessage);
                    }
                    catch
                    {
                        return Json(data: new { message = "Not a valid email", status = false });
                    }
                }

                objContact.CREATED_DATE = BdCurrentTime;
                _context.CONTACTS.Add(objContact);
                await _context.SaveChangesAsync();
                //HttpContext.Session.Remove(Constant.myContact);
                return Json(data: new { message = "Message Sent Successfully", status = true });
            }
            catch
            {
                return Json(data: new { message = "Something went wrong", status = false });
            }
        }
        //// GET: MY_PROFILE/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null || _context.MY_PROFILE == null)
        //    {
        //        return NotFound();
        //    }

        //    var mY_PROFILE = await _context.MY_PROFILE
        //        .FirstOrDefaultAsync(m => m.AUTO_ID == id);
        //    if (mY_PROFILE == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(mY_PROFILE);
        //}

        [Authorize]
        //GET: MY_PROFILE/Create
        public IActionResult Create()
        {
            return View();
        }

        //POST: MY_PROFILE/Create
        //To protect from overposting attacks, enable the specific properties you want to bind to.
        //For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MY_PROFILE mY_PROFILE)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var files = Request.Form.Files.FirstOrDefault();
                    if (files != null)
                    {
                        const string rootFolder = @"Images\About";
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
                        mY_PROFILE.PROFILE_IMAGE = uploadPath;

                        _context.Add(mY_PROFILE);
                        await _context.SaveChangesAsync();

                    }
                    _context.Add(mY_PROFILE);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
                //DO SOMTHING
            }
            return View(mY_PROFILE);
        }

        // GET: MY_PROFILE/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.MY_PROFILE == null)
            {
                return NotFound();
            }

            var mY_PROFILE = await _context.MY_PROFILE.FindAsync(id);
            if (mY_PROFILE == null)
            {
                return NotFound();
            }
            ViewData["rootPath"] = _webHostEnvironment.WebRootPath;
            return View(mY_PROFILE);
        }

        // POST: MY_PROFILE/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,MY_PROFILE mY_PROFILE)
        {
            if (id != mY_PROFILE.AUTO_ID)
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
                        const string rootFolder = @"Images\About";
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
                        if (System.IO.File.Exists(mY_PROFILE?.PROFILE_IMAGE))
                            System.IO.File.Delete(mY_PROFILE.PROFILE_IMAGE);
                        //saving the file
                        await Utility.SaveFileAsync(uploadPath, files);
                        mY_PROFILE.PROFILE_IMAGE = uploadPath;
                    }

                    _context.Update(mY_PROFILE);
                    await _context.SaveChangesAsync();

                    //HttpContext.Session.Remove(Constant.myProfile);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MY_PROFILEExists(mY_PROFILE.AUTO_ID))
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
            return View(mY_PROFILE);
        }

        // GET: MY_PROFILE/Delete/5
        //[Authorize]
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null || _context.MY_PROFILE == null)
        //    {
        //        return NotFound();
        //    }

        //    var mY_PROFILE = await _context.MY_PROFILE
        //        .FirstOrDefaultAsync(m => m.AUTO_ID == id);
        //    if (mY_PROFILE == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(mY_PROFILE);
        //}

        //// POST: MY_PROFILE/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[Authorize]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    if (_context.MY_PROFILE == null)
        //    {
        //        return Problem("Entity set 'ApplicationDbContext.MY_PROFILE'  is null.");
        //    }
        //    var mY_PROFILE = await _context.MY_PROFILE.FindAsync(id);
        //    if (mY_PROFILE != null)
        //    {
        //        _context.MY_PROFILE.Remove(mY_PROFILE);
        //    }
            
        //    await _context.SaveChangesAsync();

        //    //HttpContext.Session.Remove(Constant.myProfile);
        //    return RedirectToAction(nameof(Index));
        //}

        private bool MY_PROFILEExists(int id)
        {
          return (_context.MY_PROFILE?.Any(e => e.AUTO_ID == id)).GetValueOrDefault();
        }
    }
}
