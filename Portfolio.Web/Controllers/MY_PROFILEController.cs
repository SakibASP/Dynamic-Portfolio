using System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Portfolio.Application.Common;
using Portfolio.Application.DTOs;
using Portfolio.Application.Services;
using Portfolio.Domain;
using Portfolio.Web.Common;
using Serilog;
using X.PagedList;

namespace Portfolio.Web.Controllers;

public class MY_PROFILEController(
    IWebHostEnvironment webHostEnvironment,
    IProfileService profiles,
    IEmailSenderRelay email) : BaseController
{
    private readonly IProfileService _profiles = profiles;
    private readonly IEmailSenderRelay _email = email;
    private readonly IWebHostEnvironment _env = webHostEnvironment;

    [Authorize]
    public async Task<IActionResult> Index()
    {
        ViewData["rootPath"] = _env.WebRootPath;
        return View(await _profiles.GetAllAsync());
    }

    public async Task<IActionResult> About()
    {
        ViewData["rootPath"] = _env.WebRootPath;
        ViewData["SKILLS"] = await _profiles.GetSkillsAsync();
        ViewData["PROFILES"] = await _profiles.GetSingleAsync();
        return View();
    }

    public async Task<IActionResult> Resume()
    {
        ViewData["PROFILES"] = await _profiles.GetSingleAsync();
        ViewData["EDUCATIONS"] = await _profiles.GetEducationsAsync();
        ViewData["EXPERIENCEs"] = await _profiles.GetExperiencesAsync();
        ViewData["DESCRIPTIONs"] = await _profiles.GetDescriptionsAsync();
        return View();
    }

    public async Task<IActionResult> Projects()
    {
        ViewData["PROFILES"] = await _profiles.GetSingleAsync();
        ViewData["PROJECTS"] = await _profiles.GetProjectsAsync();
        return View();
    }

    public async Task<IActionResult> Contact()
    {
        ViewData["PROFILES"] = await _profiles.GetSingleAsync();
        return View();
    }

    public async Task<IActionResult> Visitors(string searchString, int? page, DateTime? startDate, DateTime? endDate)
    {
        ViewBag.SearchString = searchString;
        ViewData["StartDate"] = startDate?.ToString("yyyy-MM-dd");
        ViewData["EndDate"] = endDate?.ToString("yyyy-MM-dd");
        ViewData["IsAuthenticated"] = User.Identity?.IsAuthenticated ?? false;

        const int pageSize = 20;
        int pageNumber = page ?? 1;

        var parameters = new[]
        {
            new SqlParameter("@PageNumber",   SqlDbType.Int)      { Value = pageNumber },
            new SqlParameter("@PageSize",     SqlDbType.Int)      { Value = pageSize },
            new SqlParameter("@StartDate",    SqlDbType.DateTime) { Value = (object?)startDate ?? DBNull.Value },
            new SqlParameter("@EndDate",      SqlDbType.DateTime) { Value = (object?)endDate ?? DBNull.Value },
            new SqlParameter("@SearchString", SqlDbType.NVarChar) { Value = (object?)searchString ?? DBNull.Value }
        };

        var visitors = await _profiles.GetVisitorsAsync(parameters) ?? [];
        var totalRows = visitors.FirstOrDefault()?.TotalRows ?? 0;
        ViewData["TotalRecords"] = totalRows;

        return View(new StaticPagedList<VisitorsViewModel>(visitors, pageNumber, pageSize, totalRows));
    }

    [HttpPost]
    public async Task<JsonResult> Contact(CONTACTS objContact)
    {
        try
        {
            if (!string.IsNullOrEmpty(objContact.EMAIL))
            {
                try
                {
                    await _email.SendContactAcknowledgementAsync(objContact);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Contact email failed for {Email}", objContact.EMAIL);
                    return Json(new { message = "Not a valid email", status = false });
                }
            }

            await _profiles.SendContactAsync(objContact, string.Empty);
            return Json(new { message = "Message Sent Successfully", status = true });
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Contact failed");
            return Json(new { message = "Something went wrong", status = false });
        }
    }

    [Authorize]
    public IActionResult Create() => View();

    [HttpPost, Authorize, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MY_PROFILE mY_PROFILE)
    {
        if (!ModelState.IsValid) return View(mY_PROFILE);
        try
        {
            await SaveUploadedProfileImageAsync(mY_PROFILE, existingPath: null);
            await _profiles.CreateAsync(mY_PROFILE, CurrentUserName);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex) { LogAndFlash(ex); }
        return View(mY_PROFILE);
    }

    [Authorize]
    public async Task<IActionResult> Edit(int? id)
    {
        var profile = await _profiles.GetByIdAsync(id);
        if (profile == null) return NotFound();
        ViewData["rootPath"] = _env.WebRootPath;
        return View(profile);
    }

    [HttpPost, Authorize, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, MY_PROFILE mY_PROFILE)
    {
        if (id != mY_PROFILE.AUTO_ID) return NotFound();
        if (!ModelState.IsValid) return View(mY_PROFILE);

        try
        {
            await SaveUploadedProfileImageAsync(mY_PROFILE, existingPath: mY_PROFILE.PROFILE_IMAGE);
            await _profiles.UpdateAsync(mY_PROFILE, CurrentUserName);
        }
        catch (Exception ex) { LogAndFlash(ex); }
        return RedirectToAction(nameof(Index));
    }

    private async Task SaveUploadedProfileImageAsync(MY_PROFILE profile, string? existingPath)
    {
        var file = Request.Form.Files.FirstOrDefault();
        if (file == null) return;

        var directory = Path.Combine(_env.WebRootPath, "Images", "About");
        if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

        var stamp = DateTime.Now.ToString("yyyyMMddHHmmss");
        var uploadPath = Path.Combine(directory, $"{stamp}_{file.FileName}");

        if (!string.IsNullOrEmpty(existingPath) && System.IO.File.Exists(existingPath))
            System.IO.File.Delete(existingPath);

        await Utility.SaveFileAsync(uploadPath, file);
        profile.PROFILE_IMAGE = uploadPath;
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
