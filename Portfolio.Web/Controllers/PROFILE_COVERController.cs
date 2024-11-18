using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.Interfaces;
using Portfolio.Models;
using Portfolio.Utils;
using Portfolio.Web.Common;
using Serilog;

namespace Portfolio.Web.Controllers
{
    [Authorize]
    public class PROFILE_COVERController(IProfileCoverRepo cover, IWebHostEnvironment webHostEnvironment) : BaseController
    {
        private readonly IProfileCoverRepo _cover = cover;
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

        // GET: PROFILE_COVER
        public async Task<IActionResult> Index()
        {
            ViewData["rootPath"] = _webHostEnvironment.WebRootPath;
            return View(await _cover.GetAllProfileCoversAsync());
        }

        // GET: PROFILE_COVER/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pROFILE_COVER = await _cover.GetProfileCoverByIdAsync(id);
            if (pROFILE_COVER == null)
            {
                return NotFound();
            }
            ViewData["rootPath"] = _webHostEnvironment.WebRootPath;
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

                    var saveParameter = GenerateParameter.SingleModel(pROFILE_COVER, User.Identity!.Name, BdCurrentTime);
                    await _cover.AddProfileCoverAsync(saveParameter);
                    return RedirectToAction(nameof(Index));

                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData[Constant.Error] = Constant.ErrorMessage;
                Log.Error(ex, $"I am from {ControllerContext.ActionDescriptor.ControllerName} {ControllerContext.ActionDescriptor.MethodInfo.Name}... {User.Identity?.Name}");
            }
            return View(pROFILE_COVER);
        }

        // GET: PROFILE_COVER/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pROFILE_COVER = await _cover.GetProfileCoverByIdAsync(id);
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

                        var saveParameter = GenerateParameter.SingleModel(pROFILE_COVER, User.Identity!.Name, BdCurrentTime);
                        await _cover.UpdateProfileCoverAsync(saveParameter);
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    TempData[Constant.Error] = Constant.ErrorMessage;
                    Log.Error(ex, $"I am from {ControllerContext.ActionDescriptor.ControllerName} {ControllerContext.ActionDescriptor.MethodInfo.Name}... {User.Identity?.Name}");
                }
                return RedirectToAction(nameof(Index));
            }
            return View(pROFILE_COVER);
        }
    }
}
