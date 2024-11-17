using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Portfolio.Interfaces;
using Portfolio.Models;
using Portfolio.Utils;
using Portfolio.Web.Common;
using Serilog;

namespace Portfolio.Web.Controllers
{
    [Authorize]
    public class DESCRIPTIONsController(IDescriptionRepo description, IProfileRepo profile, IDescriptionTypeRepo type) : BaseController
    {
        private readonly IDescriptionRepo _description = description;
        private readonly IProfileRepo _profile = profile;
        private readonly IDescriptionTypeRepo _type = type;

        // GET: DESCRIPTIONs
        public async Task<IActionResult> Index()
        {
            var descriptions = await _description.GetAllDescriptionsAsync();
            return View(descriptions);
        }

        // GET: DESCRIPTIONs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dESCRIPTION = await _description.GetDescriptionByIdAsync(id);
            if (dESCRIPTION == null)
            {
                return NotFound();
            }

            return View(dESCRIPTION);
        }

        // GET: DESCRIPTIONs/Create
        public async Task<IActionResult> Create()
        {
            ViewData["TYPE_ID"] = new SelectList(await _type.GetAllDescriptionTypesAsync(), "AUTO_ID", "TYPE");
            ViewData["PROJECT_ID"] = new SelectList(await _profile.GetProjectsAsync(), "AUTO_ID", "PROJECT_NAME");
            ViewData["EXPERIENCE_ID"] = new SelectList(await _profile.GetExperiencesAsync(), "AUTO_ID", "INSTITUTE");
            return View();
        }

        // POST: DESCRIPTIONs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DESCRIPTION dESCRIPTION)
        {
            try
            {
                var saveParameter = GenerateParameter.SingleModel<DESCRIPTION>(dESCRIPTION, User.Identity!.Name, BdCurrentTime);
                await _description.AddDescriptionAsync(saveParameter);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData[Constant.Error] = Constant.ErrorMessage;
                Log.Error(ex, $"I am from {ControllerContext.ActionDescriptor.ControllerName} {ControllerContext.ActionDescriptor.MethodInfo.Name}... {User.Identity?.Name}");
            }

            ViewData["TYPE_ID"] = new SelectList(await _type.GetAllDescriptionTypesAsync(), "AUTO_ID", "TYPE", dESCRIPTION.TYPE_ID);
            ViewData["PROJECT_ID"] = new SelectList(await _profile.GetProjectsAsync(), "AUTO_ID", "PROJECT_NAME", dESCRIPTION.PROJECT_ID);
            ViewData["EXPERIENCE_ID"] = new SelectList(await _profile.GetExperiencesAsync(), "AUTO_ID", "INSTITUTE", dESCRIPTION.EXPERIENCE_ID);
            return View(dESCRIPTION);
        }

        // GET: DESCRIPTIONs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dESCRIPTION = await _description.GetDescriptionByIdAsync(id);
            if (dESCRIPTION == null)
            {
                return NotFound();
            }

            ViewData["TYPE_ID"] = new SelectList(await _type.GetAllDescriptionTypesAsync(), "AUTO_ID", "TYPE", dESCRIPTION.TYPE_ID);
            ViewData["PROJECT_ID"] = new SelectList(await _profile.GetProjectsAsync(), "AUTO_ID", "PROJECT_NAME", dESCRIPTION.PROJECT_ID);
            ViewData["EXPERIENCE_ID"] = new SelectList(await _profile.GetExperiencesAsync(), "AUTO_ID", "INSTITUTE", dESCRIPTION.EXPERIENCE_ID);
            return View(dESCRIPTION);
        }

        // POST: DESCRIPTIONs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DESCRIPTION dESCRIPTION)
        {
            if (id != dESCRIPTION.AUTO_ID)
            {
                return NotFound();
            }

            try
            {
                var saveParameter = GenerateParameter.SingleModel<DESCRIPTION>(dESCRIPTION, User.Identity!.Name, BdCurrentTime);
                await _description.UpdateDescriptionAsync(saveParameter);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData[Constant.Error] = Constant.ErrorMessage;
                Log.Error(ex, $"I am from {ControllerContext.ActionDescriptor.ControllerName} {ControllerContext.ActionDescriptor.MethodInfo.Name}... {User.Identity?.Name}");
            }

            ViewData["TYPE_ID"] = new SelectList(await _type.GetAllDescriptionTypesAsync(), "AUTO_ID", "TYPE", dESCRIPTION.TYPE_ID);
            ViewData["PROJECT_ID"] = new SelectList(await _profile.GetProjectsAsync(), "AUTO_ID", "PROJECT_NAME", dESCRIPTION.PROJECT_ID);
            ViewData["EXPERIENCE_ID"] = new SelectList(await _profile.GetExperiencesAsync(), "AUTO_ID", "INSTITUTE", dESCRIPTION.EXPERIENCE_ID);
            return View(dESCRIPTION);
        }

        // GET: DESCRIPTIONs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dESCRIPTION = await _description.GetDescriptionByIdAsync(id);
            if (dESCRIPTION == null)
            {
                return NotFound();
            }

            return View(dESCRIPTION);
        }

        // POST: DESCRIPTIONs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _description.RemoveDescriptionAsync(id);
            }
            catch (Exception ex)
            {
                TempData[Constant.Error] = Constant.ErrorMessage;
                Log.Error(ex, $"I am from {ControllerContext.ActionDescriptor.ControllerName} {ControllerContext.ActionDescriptor.MethodInfo.Name}... {User.Identity?.Name}");
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
