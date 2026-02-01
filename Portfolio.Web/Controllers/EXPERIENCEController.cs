using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Interfaces;
using Portfolio.Models;
using Portfolio.Utils;
using Portfolio.Web.Common;
using Serilog;

namespace Portfolio.Web.Controllers;

[Authorize]
public class EXPERIENCEController(IExperienceRepo experience) : BaseController
{
    private readonly IExperienceRepo _experience = experience;

    // GET: EXPERIENCE
    public async Task<IActionResult> Index()
    {
        var experiences = await _experience.GetAllExperiencesAsync();
          return View(experiences);
    }

    // GET: EXPERIENCE/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var eXPERIENCE = await _experience.GetExperienceByIdAsync(id);
        if (eXPERIENCE == null)
        {
            return NotFound();
        }

        return View(eXPERIENCE);
    }

    // GET: EXPERIENCE/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: EXPERIENCE/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(EXPERIENCE eXPERIENCE)
    {
        try
        {
            var saveParameter = GenerateParameter.SingleModel(eXPERIENCE, User.Identity!.Name, BdCurrentTime);
            await _experience.AddExperienceAsync(saveParameter);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData[Constant.Error] = Constant.ErrorMessage;
            Log.Error(ex, $"I am from {ControllerContext.ActionDescriptor.ControllerName} {ControllerContext.ActionDescriptor.MethodInfo.Name}... {User.Identity?.Name}");
        }
        return View(eXPERIENCE);
    }

    // GET: EXPERIENCE/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var eXPERIENCE = await _experience.GetExperienceByIdAsync(id);
        if (eXPERIENCE == null)
        {
            return NotFound();
        }
        return View(eXPERIENCE);
    }

    // POST: EXPERIENCE/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EXPERIENCE eXPERIENCE)
    {
        if (id != eXPERIENCE.AUTO_ID)
        {
            return NotFound();
        }

        try
        {
            var saveParameter = GenerateParameter.SingleModel(eXPERIENCE, User.Identity!.Name, BdCurrentTime);
            await _experience.UpdateExperienceAsync(saveParameter);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData[Constant.Error] = Constant.ErrorMessage;
            Log.Error(ex, $"I am from {ControllerContext.ActionDescriptor.ControllerName} {ControllerContext.ActionDescriptor.MethodInfo.Name}... {User.Identity?.Name}");
        }
        return View(eXPERIENCE);
    }

    // GET: EXPERIENCE/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var eXPERIENCE = await _experience.GetExperienceByIdAsync(id);
        if (eXPERIENCE == null)
        {
            return NotFound();
        }

        return View(eXPERIENCE);
    }

    // POST: EXPERIENCE/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            await _experience.RemoveExperienceAsync(id);
        }
        catch (Exception ex)
        {
            TempData[Constant.Error] = Constant.ErrorMessage;
            Log.Error(ex, $"I am from {ControllerContext.ActionDescriptor.ControllerName} {ControllerContext.ActionDescriptor.MethodInfo.Name}... {User.Identity?.Name}");
        }

        return RedirectToAction(nameof(Index));
    }

}
