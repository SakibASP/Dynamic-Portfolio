using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Application.Common;
using Portfolio.Application.Services;
using Portfolio.Domain;
using Portfolio.Web.Common;
using Serilog;

namespace Portfolio.Web.Controllers;

[Authorize]
public class CONTACTSController(IContactsService contacts) : BaseController
{
    private readonly IContactsService _contacts = contacts;

    public async Task<IActionResult> Index() =>
        View(await _contacts.GetAllAsync());

    public async Task<IActionResult> PendingIndex() =>
        View(await _contacts.GetPendingAsync());

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var contact = await _contacts.GetByIdAsync(id);
        if (contact == null) return NotFound();

        if (contact.IsConfirmed != 1)
        {
            try { await _contacts.ConfirmAsync(contact, CurrentUserName); }
            catch (Exception ex) { LogAndFlash(ex); }
        }
        return View(contact);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var contact = await _contacts.GetByIdAsync(id);
        return contact == null ? NotFound() : View(contact);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CONTACTS cONTACTS)
    {
        if (id != cONTACTS.AUTO_ID) return NotFound();
        try
        {
            await _contacts.UpdateAsync(cONTACTS, CurrentUserName);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex) { LogAndFlash(ex); }
        return View(cONTACTS);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var contact = await _contacts.GetByIdAsync(id);
        return contact == null ? NotFound() : View(contact);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try { await _contacts.RemoveAsync(id); }
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
