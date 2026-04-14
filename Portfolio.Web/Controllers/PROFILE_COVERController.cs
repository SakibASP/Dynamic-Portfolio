using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Application.Common;
using Portfolio.Application.Services;
using Portfolio.Domain;
using Portfolio.Web.Common;
using Serilog;

namespace Portfolio.Web.Controllers;

[Authorize]
public class PROFILE_COVERController(
    IProfileCoverService covers,
    IWebHostEnvironment webHostEnvironment) : BaseController
{
    private readonly IProfileCoverService _covers = covers;
    private readonly IWebHostEnvironment _env = webHostEnvironment;

    public async Task<IActionResult> Index()
    {
        ViewData["rootPath"] = _env.WebRootPath;
        return View(await _covers.GetAllAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var cover = await _covers.GetByIdAsync(id);
        if (cover == null) return NotFound();
        ViewData["rootPath"] = _env.WebRootPath;
        return View(cover);
    }

    public IActionResult Create() => View();

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PROFILE_COVER pROFILE_COVER)
    {
        try
        {
            await SaveUploadedCoverImageAsync(pROFILE_COVER);
            await _covers.CreateAsync(pROFILE_COVER, CurrentUserName);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex) { LogAndFlash(ex); }
        return View(pROFILE_COVER);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var cover = await _covers.GetByIdAsync(id);
        if (cover == null) return NotFound();
        ViewData["rootPath"] = _env.WebRootPath;
        return View(cover);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, PROFILE_COVER pROFILE_COVER)
    {
        if (id != pROFILE_COVER.AUTO_ID) return NotFound();
        if (!ModelState.IsValid) return View(pROFILE_COVER);

        try
        {
            var old = pROFILE_COVER.COVER_IMAGE;
            if (await SaveUploadedCoverImageAsync(pROFILE_COVER) && !string.IsNullOrEmpty(old) && System.IO.File.Exists(old))
                System.IO.File.Delete(old);

            await _covers.UpdateAsync(pROFILE_COVER, CurrentUserName);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex) { LogAndFlash(ex); }
        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> SaveUploadedCoverImageAsync(PROFILE_COVER cover)
    {
        var file = Request.Form.Files.FirstOrDefault();
        if (file == null) return false;

        var directory = Path.Combine(_env.WebRootPath, "Images", "Cover");
        if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

        var stamp = DateTime.Now.ToString("yyyyMMddHHmmss");
        var uploadPath = Path.Combine(directory, $"{stamp}_{file.FileName}");
        await Utility.SaveFileAsync(uploadPath, file);
        cover.COVER_IMAGE = uploadPath;
        return true;
    }

    private void LogAndFlash(Exception ex)
    {
        TempData[Constant.Error] = Constant.ErrorMessage;
        Log.Error(ex, "{Controller}.{Action} by {User}",
            ControllerContext.ActionDescriptor.ControllerName,
            ControllerContext.ActionDescriptor.ActionName,
            CurrentUserName);
    }
}
