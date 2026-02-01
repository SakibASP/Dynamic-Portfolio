using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Interfaces;
using Portfolio.Models;
using Portfolio.Utils;
using Portfolio.Web.Common;
using Serilog;

namespace Portfolio.Web.Controllers;

[Authorize]
public class MY_SKILLController(ISkillsRepo skills) : BaseController
{
    private readonly ISkillsRepo _skills = skills;

    // GET: MY_SKILL
    public async Task<IActionResult> Index()
    {
        var skills = await _skills.GetAllSkillsAsync();
        return View(skills);
    }

    // GET: MY_SKILL/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var mY_SKILLS = await _skills.GetSkillByIdAsync(id);
        if (mY_SKILLS == null)
        {
            return NotFound();
        }

        return View(mY_SKILLS);
    }

    // GET: MY_SKILL/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: MY_SKILL/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MY_SKILLS mY_SKILLS)
    {
        try
        {
            var saveParameter = GenerateParameter.SingleModel(mY_SKILLS, User.Identity!.Name, BdCurrentTime);
            await _skills.AddSkillAsync(saveParameter);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData[Constant.Error] = Constant.ErrorMessage;
            Log.Error(ex, $"I am from {ControllerContext.ActionDescriptor.ControllerName} {ControllerContext.ActionDescriptor.MethodInfo.Name}... {User.Identity?.Name}");
        }
        return View(mY_SKILLS);
    }

    // GET: MY_SKILL/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var mY_SKILLS = await _skills.GetSkillByIdAsync(id);
        if (mY_SKILLS == null)
        {
            return NotFound();
        }
        return View(mY_SKILLS);
    }

    // POST: MY_SKILL/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, MY_SKILLS mY_SKILLS)
    {
        if (id != mY_SKILLS.AUTO_ID)
        {
            return NotFound();
        }

        try
        {
            var saveParameter = GenerateParameter.SingleModel(mY_SKILLS, User.Identity!.Name, BdCurrentTime);
            await _skills.UpdateSkillAsync(saveParameter);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData[Constant.Error] = Constant.ErrorMessage;
            Log.Error(ex, $"I am from {ControllerContext.ActionDescriptor.ControllerName} {ControllerContext.ActionDescriptor.MethodInfo.Name}... {User.Identity?.Name}");
        }
        return View(mY_SKILLS);
    }

    // GET: MY_SKILL/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var mY_SKILLS = await _skills.GetSkillByIdAsync(id);
        if (mY_SKILLS == null)
        {
            return NotFound();
        }

        return View(mY_SKILLS);
    }

    // POST: MY_SKILL/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            await _skills.RemoveSkillAsync(id);
        }
        catch (Exception ex)
        {
            TempData[Constant.Error] = Constant.ErrorMessage;
            Log.Error(ex, $"I am from {ControllerContext.ActionDescriptor.ControllerName} {ControllerContext.ActionDescriptor.MethodInfo.Name}... {User.Identity?.Name}");
        }

        return RedirectToAction(nameof(Index));
    }
}
