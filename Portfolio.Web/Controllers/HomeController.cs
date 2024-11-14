using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.Web.Common;
using Portfolio.Web.Data;
using Portfolio.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Http.Extensions;
using UAParser;
using Microsoft.AspNetCore.Hosting;
using Portfolio.Web.Models;

namespace Portfolio.Web.Controllers
{
    public class HomeController(ApplicationDbContext context, IWebHostEnvironment environment) : BaseController
    {
        private readonly IWebHostEnvironment _hostEnvironment = environment;
        private readonly ApplicationDbContext _context = context;

        public async Task<IActionResult> Index()
        {
            if (User.Identity!.IsAuthenticated)
            {
                //if (S_CONTACTS is not null)
                //    myMessage = S_CONTACTS!.Where(x => x.IsConfirmed == null).ToList();
                //else
                var myMessage = await _context.CONTACTS.Where(x => x.IsConfirmed == null || x.IsConfirmed == 0).ToListAsync();
                ViewData["Message"] = myMessage == null ? "" : myMessage.Count.ToString();
            }

            //if (S_PROFILE_COVER is not null)
            //    Cover = S_PROFILE_COVER!.FirstOrDefault();
            //else
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