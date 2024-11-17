using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.Web.Common;
using Portfolio.Web.Data;
using System.Diagnostics;
using Portfolio.Web.Models;
using Portfolio.Repositories.Data;

namespace Portfolio.Web.Controllers
{
    public class HomeController(PortfolioDbContext context, IWebHostEnvironment environment) : BaseController
    {
        private readonly IWebHostEnvironment _hostEnvironment = environment;
        private readonly PortfolioDbContext _context = context;

        public async Task<IActionResult> Index()
        {
            if (User.Identity!.IsAuthenticated)
            {
                var myMessage = await _context.CONTACTS.Where(x => x.IsConfirmed == null || x.IsConfirmed == 0).ToListAsync();
                ViewData["Message"] = myMessage == null ? "" : myMessage.Count.ToString();
            }

            var cover = await _context.PROFILE_COVER.FirstOrDefaultAsync();
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