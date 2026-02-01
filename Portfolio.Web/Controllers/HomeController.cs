using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Portfolio.Interfaces;
using Portfolio.ViewModels;
using Portfolio.Web.Common;
using Portfolio.Web.Models;
using System.Diagnostics;

namespace Portfolio.Web.Controllers;

public class HomeController(IHomeRepo home, IProfileCoverRepo cover, IWebHostEnvironment environment) : BaseController
{
    private readonly IWebHostEnvironment _hostEnvironment = environment;
    private readonly IHomeRepo _home = home;
    private readonly IProfileCoverRepo _cover = cover;

    public async Task<IActionResult> Index()
    {
        if (User.Identity!.IsAuthenticated)
        {
            var messageCount = await _home.GetUnreadMessagesCountAsync();
            ViewData["Message"] = messageCount == null ? "" : messageCount.ToString();
        }

        var coverList = await _cover.GetAllProfileCoversAsync();
        var cover = coverList.FirstOrDefault();
        var filePath = Utility.GetFilePathOfCV(_hostEnvironment);

        ViewBag.FilePath = filePath;
        ViewBag.Name = "Md. Sakibur Rahman";
        ViewBag.Bio = "I am a professiona Software Developer from Khulna, Bangladesh";
        ViewBag.Cover = cover;
        ViewData["rootPath"] = _hostEnvironment.WebRootPath;

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [HttpPost]
    public async Task<JsonResult> SaveLocation([FromBody] LocationRequest request)
    {
        if (request is null) return Json(new { success = false });
        request.IPAddress = HttpContext.Connection.RemoteIpAddress!.ToString();
        request.UserAgent = HttpContext.Request.Headers.UserAgent.ToString();
        request.OperatingSystem = Utility.GetVisitorsDeviceInfo(request.UserAgent).OperatingSystem!;
        request.VisitTime = BdCurrentTime;
        var result = await _home.SaveLocationAsync(request);
        if (result) return Json(new { success = true });
        return Json(new { success = false });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}