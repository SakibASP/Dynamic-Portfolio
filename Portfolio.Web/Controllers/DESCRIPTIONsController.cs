using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Portfolio.Application.Common;
using Portfolio.Application.Services;
using Portfolio.Domain;
using Portfolio.Web.Common;
using Serilog;

namespace Portfolio.Web.Controllers;

[Authorize]
public class DESCRIPTIONsController(
    IDescriptionService descriptions,
    IDescriptionTypeService types,
    IProjectService projects,
    IExperienceService experiences) : BaseController
{
    private readonly IDescriptionService _descriptions = descriptions;
    private readonly IDescriptionTypeService _types = types;
    private readonly IProjectService _projects = projects;
    private readonly IExperienceService _experiences = experiences;

    public async Task<IActionResult> Index() => View(await _descriptions.GetAllAsync());

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var item = await _descriptions.GetByIdAsync(id);
        return item == null ? NotFound() : View(item);
    }

    public async Task<IActionResult> Create()
    {
        await PopulateLookupsAsync(null);
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(DESCRIPTION dESCRIPTION)
    {
        try
        {
            await _descriptions.CreateAsync(dESCRIPTION, CurrentUserName);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex) { LogAndFlash(ex); }

        await PopulateLookupsAsync(dESCRIPTION);
        return View(dESCRIPTION);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var item = await _descriptions.GetByIdAsync(id);
        if (item == null) return NotFound();

        await PopulateLookupsAsync(item);
        return View(item);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, DESCRIPTION dESCRIPTION)
    {
        if (id != dESCRIPTION.AUTO_ID) return NotFound();
        try
        {
            await _descriptions.UpdateAsync(dESCRIPTION, CurrentUserName);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex) { LogAndFlash(ex); }

        await PopulateLookupsAsync(dESCRIPTION);
        return View(dESCRIPTION);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var item = await _descriptions.GetByIdAsync(id);
        return item == null ? NotFound() : View(item);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try { await _descriptions.RemoveAsync(id); }
        catch (Exception ex) { LogAndFlash(ex); }
        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateLookupsAsync(DESCRIPTION? current)
    {
        ViewData["TYPE_ID"]       = new SelectList(await _types.GetAllAsync(),       "AUTO_ID", "TYPE",         current?.TYPE_ID);
        ViewData["PROJECT_ID"]    = new SelectList(await _projects.GetAllAsync(),    "AUTO_ID", "PROJECT_NAME", current?.PROJECT_ID);
        ViewData["EXPERIENCE_ID"] = new SelectList(await _experiences.GetAllAsync(), "AUTO_ID", "INSTITUTE",    current?.EXPERIENCE_ID);
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
