using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Interfaces;
using Portfolio.Models;
using Portfolio.Utils;
using Portfolio.Web.Common;
using Serilog;

namespace Portfolio.Web.Controllers;

[Authorize]
public class EDUCATIONController(IEducationRepo education) : BaseController
{
    private readonly IEducationRepo _education = education;

    // GET: EDUCATION
    public async Task<IActionResult> Index()
    {
        var educations = await _education.GetAllEducationsAsync();
        return View(educations);
    }

    // GET: EDUCATION/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var eDUCATION = await _education.GetEducationByIdAsync(id);
        if (eDUCATION == null)
        {
            return NotFound();
        }

        return View(eDUCATION);
    }

    // GET: EDUCATION/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: EDUCATION/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(EDUCATION eDUCATION)
    {
        try
        {
            var saveParameter = GenerateParameter.SingleModel(eDUCATION, User.Identity!.Name, BdCurrentTime);
            await _education.AddEducationAsync(saveParameter);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData[Constant.Error] = Constant.ErrorMessage;
            Log.Error(ex, $"I am from {ControllerContext.ActionDescriptor.ControllerName} {ControllerContext.ActionDescriptor.MethodInfo.Name}... {User.Identity?.Name}");
        }
        return View(eDUCATION);
    }

    // GET: EDUCATION/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var eDUCATION = await _education.GetEducationByIdAsync(id);
        if (eDUCATION == null)
        {
            return NotFound();
        }
        return View(eDUCATION);
    }

    // POST: EDUCATION/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EDUCATION eDUCATION)
    {
        if (id != eDUCATION.AUTO_ID)
        {
            return NotFound();
        }

        try
        {
            var saveParameter = GenerateParameter.SingleModel(eDUCATION, User.Identity!.Name, BdCurrentTime);
            await _education.UpdateEducationAsync(saveParameter);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData[Constant.Error] = Constant.ErrorMessage;
            Log.Error(ex, $"I am from {ControllerContext.ActionDescriptor.ControllerName} {ControllerContext.ActionDescriptor.MethodInfo.Name}... {User.Identity?.Name}");
        }
        return View(eDUCATION);
    }

    // GET: EDUCATION/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var eDUCATION = await _education.GetEducationByIdAsync(id);
        if (eDUCATION == null)
        {
            return NotFound();
        }

        return View(eDUCATION);
    }

    // POST: EDUCATION/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            await _education.RemoveEducationAsync(id);
        }
        catch (Exception ex)
        {
            TempData[Constant.Error] = Constant.ErrorMessage;
            Log.Error(ex, $"I am from {ControllerContext.ActionDescriptor.ControllerName} {ControllerContext.ActionDescriptor.MethodInfo.Name}... {User.Identity?.Name}");
        }

        return RedirectToAction(nameof(Index));
    }
}
