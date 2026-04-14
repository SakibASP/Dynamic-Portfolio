using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Application.Common;
using Portfolio.Application.Services;
using Portfolio.Domain;
using Portfolio.Web.Common;
using Serilog;

namespace Portfolio.Web.Controllers;

[Authorize]
public class EXPERIENCEController(IExperienceService experiences) : BaseController
{
    private readonly IExperienceService _experiences = experiences;

    public async Task<IActionResult> Index() => View(await _experiences.GetAllAsync());

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var item = await _experiences.GetByIdAsync(id);
        return item == null ? NotFound() : View(item);
    }

    public IActionResult Create() => View();

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(EXPERIENCE eXPERIENCE)
    {
        try
        {
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
        return item == null ? NotFound() : View(item);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EXPERIENCE eXPERIENCE)
    {
        if (id != eXPERIENCE.AUTO_ID) return NotFound();
        try
        {
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
        try { await _experiences.RemoveAsync(id); }
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
