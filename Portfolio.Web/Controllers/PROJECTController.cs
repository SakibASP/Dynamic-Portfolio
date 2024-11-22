using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Interfaces;
using Portfolio.Models;
using Portfolio.Utils;
using Portfolio.Web.Common;
using Serilog;

namespace Portfolio.Web.Controllers
{
    public class PROJECTSController(IProjectRepo project) : BaseController
    {
        private readonly IProjectRepo _project = project;

        // GET: SERVICES
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var projects = await _project.GetAllProjectsAsync();
            return View(projects);
        }

        // GET: SERVICES/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var PROJECTS = await _project.GetProjectByIdAsync(id);
            if (PROJECTS == null)
            {
                return NotFound();
            }
            ViewData["DESCRIPTIONs"] = await _project.GetDescriptionByProjectIdAsync(id);

            return View(PROJECTS);
        }

        [Authorize]
        // GET: PROJECTS/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PROJECTS/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PROJECTS pROJECTS)
        {
            try
            {
                var imgFile = Request.Form.Files.FirstOrDefault();
                if (imgFile != null)
                {
                    pROJECTS.LOGO = await Utility.GetImageBytes(pROJECTS.LOGO, Request.Form.Files);
                }

                var saveParameter = GenerateParameter.SingleModel(pROJECTS, User.Identity?.Name, BdCurrentTime);
                await _project.AddProjectAsync(saveParameter);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData[Constant.Error] = Constant.ErrorMessage;
                Log.Error(ex, $"I am from {ControllerContext.ActionDescriptor.ControllerName} {ControllerContext.ActionDescriptor.MethodInfo.Name}... {User.Identity?.Name}");
            }
            return View(pROJECTS);
        }

        // GET: SERVICES/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var PROJECTS = await _project.GetProjectByIdAsync(id);
            if (PROJECTS == null)
            {
                return NotFound();
            }
            return View(PROJECTS);
        }

        // POST: PROJECTS/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PROJECTS pROJECTS)
        {
            if (id != pROJECTS.AUTO_ID)
            {
                return NotFound();
            }
            try
            {
                var imgFile = Request.Form.Files.FirstOrDefault();
                if (imgFile != null)
                {
                    pROJECTS.LOGO = await Utility.GetImageBytes(pROJECTS.LOGO, Request.Form.Files);
                }

                var saveParameter = GenerateParameter.SingleModel(pROJECTS, User.Identity?.Name, BdCurrentTime);
                await _project.UpdateProjectAsync(saveParameter);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData[Constant.Error] = Constant.ErrorMessage;
                Log.Error(ex, $"I am from {ControllerContext.ActionDescriptor.ControllerName} {ControllerContext.ActionDescriptor.MethodInfo.Name}... {User.Identity?.Name}");
            }
            return View(pROJECTS);
        }

        // GET: PROJECTS/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var PROJECTS = await _project.GetProjectByIdAsync(id);
            if (PROJECTS == null)
            {
                return NotFound();
            }

            return View(PROJECTS);
        }

        // POST: PROJECTS/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _project.RemoveProjectAsync(id);
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
