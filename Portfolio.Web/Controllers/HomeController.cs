using Microsoft.AspNetCore.Mvc;
using Portfolio.Web.Common;
using System.Diagnostics;
using Portfolio.Web.Models;
using Portfolio.Interfaces;
using Tesseract;
using System.IO.Pipelines;
using Serilog;

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

        public IActionResult ProcessImageIndex()
        {
            return View();
        }

        [HttpPost]
        [Route("/api/ImageToText")]
        public async Task<IActionResult> ProcessImage()
        {
            try
            {
                var formFile = Request.Form.Files["image"];
                if (formFile == null || formFile.Length == 0)
                    return BadRequest("No image uploaded.");

                byte[] fileBytes;
                using (var memoryStream = new MemoryStream())
                {
                    await formFile.CopyToAsync(memoryStream);
                    fileBytes = memoryStream.ToArray();
                }

                string text;
                using (var stream = new MemoryStream())
                {
                    var tessDataPath = Path.Combine(_hostEnvironment.WebRootPath, "TessData");
                    // Load the image
                    using var engine = new TesseractEngine(tessDataPath, "eng", EngineMode.Default);
                    using var img = Pix.LoadFromMemory(fileBytes);
                    // Recognize text from the image
                    var page = engine.Process(img);
                    text = page.GetText();
                }

                return Ok(text);
            }
            catch(Exception ex)
            {
                Log.Error(ex, $"I am from {ControllerContext.ActionDescriptor.ControllerName} {ControllerContext.ActionDescriptor.MethodInfo.Name}... ");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
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