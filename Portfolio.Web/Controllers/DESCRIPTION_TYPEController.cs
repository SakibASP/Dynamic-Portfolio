using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Application.Common;
using Portfolio.Application.Services;
using Portfolio.Domain;
using Portfolio.Web.Common;
using Serilog;

namespace Portfolio.Web.Controllers;

[Authorize]
public class DESCRIPTION_TYPEController(IDescriptionTypeService types) : BaseController
{
    private readonly IDescriptionTypeService _types = types;

    public async Task<IActionResult> Index() => View(await _types.GetAllAsync());

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var item = await _types.GetByIdAsync(id);
        return item == null ? NotFound() : View(item);
    }

    public IActionResult Create() => View();

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(DESCRIPTION_TYPE dESCRIPTION_TYPE)
    {
        try
        {
            await _types.CreateAsync(dESCRIPTION_TYPE, CurrentUserName);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex) { LogAndFlash(ex); }
        return View(dESCRIPTION_TYPE);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var item = await _types.GetByIdAsync(id);
        return item == null ? NotFound() : View(item);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, DESCRIPTION_TYPE dESCRIPTION_TYPE)
    {
        if (id != dESCRIPTION_TYPE.AUTO_ID) return NotFound();
        try
        {
            await _types.UpdateAsync(dESCRIPTION_TYPE, CurrentUserName);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex) { LogAndFlash(ex); }
        return View(dESCRIPTION_TYPE);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var item = await _types.GetByIdAsync(id);
        return item == null ? NotFound() : View(item);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try { await _types.RemoveAsync(id); }
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
