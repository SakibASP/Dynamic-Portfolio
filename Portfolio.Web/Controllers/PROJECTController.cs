using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Application.Common;
using Portfolio.Application.Services;
using Portfolio.Domain;
using Portfolio.Web.Common;
using Serilog;

namespace Portfolio.Web.Controllers;

public class PROJECTSController(IProjectService projects) : BaseController
{
    private readonly IProjectService _projects = projects;

    [Authorize]
    public async Task<IActionResult> Index() => View(await _projects.GetAllAsync());

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var project = await _projects.GetByIdAsync(id);
        if (project == null) return NotFound();

        ViewData["DESCRIPTIONs"] = await _projects.GetDescriptionsByProjectIdAsync(id);
        return View(project);
    }

    [Authorize]
    public IActionResult Create() => View();

    [HttpPost, Authorize, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PROJECTS pROJECTS)
    {
        try
        {
            if (Request.Form.Files.FirstOrDefault() != null)
                pROJECTS.LOGO = await Utility.GetImageBytes(pROJECTS.LOGO, Request.Form.Files);

            await _projects.CreateAsync(pROJECTS, CurrentUserName);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex) { LogAndFlash(ex); }
        return View(pROJECTS);
    }

    [Authorize]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var project = await _projects.GetByIdAsync(id);
        return project == null ? NotFound() : View(project);
    }

    [HttpPost, Authorize, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, PROJECTS pROJECTS)
    {
        if (id != pROJECTS.AUTO_ID) return NotFound();
        try
        {
            if (Request.Form.Files.FirstOrDefault() != null)
                pROJECTS.LOGO = await Utility.GetImageBytes(pROJECTS.LOGO, Request.Form.Files);

            await _projects.UpdateAsync(pROJECTS, CurrentUserName);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex) { LogAndFlash(ex); }
        return View(pROJECTS);
    }

    [Authorize]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var project = await _projects.GetByIdAsync(id);
        return project == null ? NotFound() : View(project);
    }

    [HttpPost, ActionName("Delete"), Authorize, ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try { await _projects.RemoveAsync(id); }
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
