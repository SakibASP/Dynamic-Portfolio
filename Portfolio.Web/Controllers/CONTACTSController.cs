using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Interfaces;
using Portfolio.Models;
using Portfolio.Utils;
using Portfolio.Web.Common;
using Serilog;

namespace Portfolio.Web.Controllers
{
    [Authorize]
    public class CONTACTSController(IContactsRepo contact) : BaseController
    {
        private readonly IContactsRepo _contact = contact;

        // GET: CONTACTS
        public async Task<IActionResult> Index()
        {
              return View(await _contact.GetAllContactsAsync());
        }
        
        public async Task<IActionResult> PendingIndex()
        {
              return View(await _contact.GetAllPendingContactsAsync());
        }

        // GET: CONTACTS/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cONTACTS = await _contact.GetContactByIdAsync(id);

            if (cONTACTS == null)
            {
                return NotFound();
            }

            if (cONTACTS.IsConfirmed != 1)
            {
                try
                {
                    var saveParameter = GenerateParameter.SingleModel(cONTACTS, User.Identity?.Name, BdCurrentTime);
                    await _contact.ConfirmContactAsync(saveParameter);
                }
                catch (Exception ex)
                {
                    TempData[Constant.Error] = Constant.ErrorMessage;
                    Log.Error(ex, $"I am from {ControllerContext.ActionDescriptor.ControllerName} {ControllerContext.ActionDescriptor.MethodInfo.Name}... {User.Identity?.Name}");
                }
            }

            return View(cONTACTS);
        }

        // GET: CONTACTS/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cONTACTS = await _contact.GetContactByIdAsync(id);
            if (cONTACTS == null)
            {
                return NotFound();
            }
            return View(cONTACTS);
        }

        // POST: CONTACTS/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CONTACTS cONTACTS)
        {
            if (id != cONTACTS.AUTO_ID)
            {
                return NotFound();
            }

            try
            {
                var saveParameter = GenerateParameter.SingleModel(cONTACTS, User.Identity?.Name, BdCurrentTime);
                await _contact.UpdateContactAsync(saveParameter);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData[Constant.Error] = Constant.ErrorMessage;
                Log.Error(ex, $"I am from {ControllerContext.ActionDescriptor.ControllerName} {ControllerContext.ActionDescriptor.MethodInfo.Name}... {User.Identity?.Name}");
            }
            return View(cONTACTS);
        }

        // GET: CONTACTS/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cONTACTS = await _contact.GetContactByIdAsync(id);
            if (cONTACTS == null)
            {
                return NotFound();
            }

            return View(cONTACTS);
        }

        //// POST: CONTACTS/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _contact.RemoveContactAsync(id);
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
