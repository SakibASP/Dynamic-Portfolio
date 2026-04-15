using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Application.Common;
using Portfolio.Application.Services;
using Portfolio.Domain;
using Portfolio.Web.Common;
using Serilog;

namespace Portfolio.Web.Controllers;

[Authorize]
public class EXPERIENCEController(
    IExperienceService experiences,
    IWebHostEnvironment env) : BaseController
{
    private readonly IExperienceService _experiences = experiences;
    private readonly IWebHostEnvironment _env = env;

    public async Task<IActionResult> Index()
    {
        ViewData["rootPath"] = _env.WebRootPath;
        return View(await _experiences.GetAllAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var item = await _experiences.GetByIdAsync(id);
        if (item == null) return NotFound();
        ViewData["rootPath"] = _env.WebRootPath;
        return View(item);
    }

    public IActionResult Create() => View();

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(EXPERIENCE eXPERIENCE)
    {
        try
        {
            var saved = await Utility.SaveUploadedAsync(
                Request.Form.Files.FirstOrDefault(), _env, "Images", "Experience");
            if (saved != null) eXPERIENCE.LOGO = saved;

            await _experiences.CreateAsync(eXPERIENCE, CurrentUserName);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex) { LogAndFlash(ex); }
        return View(eXPERIENCE);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var item = await _experiences.GetByIdAsync(id);
        if (item == null) return NotFound();
        ViewData["rootPath"] = _env.WebRootPath;
        return View(item);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EXPERIENCE eXPERIENCE)
    {
        if (id != eXPERIENCE.AUTO_ID) return NotFound();
        try
        {
            var existing = await _experiences.GetByIdAsync(id);
            var saved = await Utility.SaveUploadedAsync(
                Request.Form.Files.FirstOrDefault(), _env, "Images", "Experience");
            if (saved != null)
            {
                Utility.DeleteIfExists(existing?.LOGO);
                eXPERIENCE.LOGO = saved;
            }
            else
            {
                eXPERIENCE.LOGO = existing?.LOGO;
            }
            await _experiences.UpdateAsync(eXPERIENCE, CurrentUserName);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex) { LogAndFlash(ex); }
        return View(eXPERIENCE);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var item = await _experiences.GetByIdAsync(id);
        return item == null ? NotFound() : View(item);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            var existing = await _experiences.GetByIdAsync(id);
            Utility.DeleteIfExists(existing?.LOGO);
            await _experiences.RemoveAsync(id);
        }
        catch (Exception ex) { LogAndFlash(ex); }
        return RedirectToAction(nameof(Index));
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
