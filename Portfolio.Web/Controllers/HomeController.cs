using Microsoft.AspNetCore.Mvc;
using Portfolio.Web.Common;
using System.Diagnostics;
using Portfolio.Web.Models;
using Portfolio.Interfaces;

namespace Portfolio.Web.Controllers
{
    public class HomeController(IHomeRepo home, IProfileCoverRepo cover, IWebHostEnvironment environment) : BaseController
    {
        private readonly IWebHostEnvironment _hostEnvironment = environment;
        private readonly IHomeRepo _home = home;
        private readonly IProfileCoverRepo _cover = cover;

        public async Task<IActionResult> Index()
        {
            if (User.Identity!.IsAuthenticated)
            {
                var _messageCount = await _home.GetUnreadMessagesCountAsync();
                ViewData["Message"] = _messageCount == null ? "" : _messageCount.ToString();
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}