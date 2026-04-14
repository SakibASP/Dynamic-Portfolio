using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Application.DTOs;
using Portfolio.Application.Services;
using Portfolio.Web.Common;
using Portfolio.Web.Models;

namespace Portfolio.Web.Controllers;

public class HomeController(
    IHomeService home,
    IProfileCoverService covers,
    IWebHostEnvironment environment) : BaseController
{
    private readonly IHomeService _home = home;
    private readonly IProfileCoverService _covers = covers;
    private readonly IWebHostEnvironment _env = environment;

    public async Task<IActionResult> Index()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            var count = await _home.GetUnreadMessagesCountAsync();
            ViewData["Message"] = count?.ToString() ?? "";
        }

        var cover = (await _covers.GetAllAsync()).FirstOrDefault();

        ViewBag.FilePath = Utility.GetFilePathOfCV(_env);
        ViewBag.Name = "Zarif Jim";
        ViewBag.Bio = "I am a professional Business Analyst from Dhaka, Bangladesh";
        ViewBag.Cover = cover;
        ViewData["rootPath"] = _env.WebRootPath;
        return View();
    }

    public IActionResult Privacy() => View();

    [HttpPost]
    public async Task<JsonResult> SaveLocation([FromBody] LocationRequest request)
    {
        if (request is null) return Json(new { success = false });

        request.IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty;
        request.UserAgent = HttpContext.Request.Headers.UserAgent.ToString();
        request.VisitTime = BdCurrentTime;

        var ok = await _home.SaveLocationAsync(request);
        return Json(new { success = ok });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() =>
        View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}
