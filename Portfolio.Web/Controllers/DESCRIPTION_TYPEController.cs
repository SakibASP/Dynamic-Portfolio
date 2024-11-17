using Microsoft.AspNetCore.Mvc;
using Portfolio.Web.Common;
using Portfolio.Models;
using Microsoft.AspNetCore.Authorization;
using Portfolio.Interfaces;
using Portfolio.Utils;
using Serilog;

namespace Portfolio.Web.Controllers
{
    [Authorize]
    public class DESCRIPTION_TYPEController(IDescriptionTypeRepo type) : BaseController
    {
        private readonly IDescriptionTypeRepo _type = type;
        // GET: DESCRIPTION_TYPE
        public async Task<IActionResult> Index()
        {
            var _types = await _type.GetAllDescriptionTypesAsync();
            return View(_types);
        }

        // GET: DESCRIPTION_TYPE/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dESCRIPTION_TYPE = await _type.GetDescriptionTypeByIdAsync(id);
            if (dESCRIPTION_TYPE == null)
            {
                return NotFound();
            }

            return View(dESCRIPTION_TYPE);
        }

        // GET: DESCRIPTION_TYPE/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DESCRIPTION_TYPE/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DESCRIPTION_TYPE dESCRIPTION_TYPE)
        {
            try
            {
                var saveParameter = GenerateParameter.SingleModel<DESCRIPTION_TYPE>(dESCRIPTION_TYPE, User.Identity?.Name, BdCurrentTime);
                await _type.AddDescriptionTypeAsync(saveParameter);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData[Constant.Error] = Constant.ErrorMessage;
                Log.Error(ex, $"I am from {ControllerContext.ActionDescriptor.ControllerName} {ControllerContext.ActionDescriptor.MethodInfo.Name}... {User.Identity?.Name}");
            }
            return View(dESCRIPTION_TYPE);
        }

        // GET: DESCRIPTION_TYPE/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dESCRIPTION_TYPE = await _type.GetDescriptionTypeByIdAsync(id);
            if (dESCRIPTION_TYPE == null)
            {
                return NotFound();
            }
            return View(dESCRIPTION_TYPE);
        }

        // POST: DESCRIPTION_TYPE/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  DESCRIPTION_TYPE dESCRIPTION_TYPE)
        {
            if (id != dESCRIPTION_TYPE.AUTO_ID)
            {
                return NotFound();
            }

            try
            {
                var saveParameter = GenerateParameter.SingleModel<DESCRIPTION_TYPE>(dESCRIPTION_TYPE, User.Identity?.Name, BdCurrentTime);
                await _type.UpdateDescriptionTypeAsync(saveParameter);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData[Constant.Error] = Constant.ErrorMessage;
                Log.Error(ex, $"I am from {ControllerContext.ActionDescriptor.ControllerName} {ControllerContext.ActionDescriptor.MethodInfo.Name}... {User.Identity?.Name}");
            }
            return View(dESCRIPTION_TYPE);
        }

        // GET: DESCRIPTION_TYPE/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dESCRIPTION_TYPE = await _type.GetDescriptionTypeByIdAsync(id);
            if (dESCRIPTION_TYPE == null)
            {
                return NotFound();
            }

            return View(dESCRIPTION_TYPE);
        }

        // POST: DESCRIPTION_TYPE/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _type.RemoveDescriptionTypeAsync(id);
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
