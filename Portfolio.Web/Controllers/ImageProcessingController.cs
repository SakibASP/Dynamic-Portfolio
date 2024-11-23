using Microsoft.AspNetCore.Mvc;
using Serilog;
using Tesseract;

namespace Portfolio.Web.Controllers
{
    public class ImageProcessingController(IWebHostEnvironment environment) : Controller
    {
        private readonly IWebHostEnvironment _hostEnvironment = environment;

        public IActionResult ProcessImageIndex()
        {
            return View();
        }

        [HttpPost]
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
            catch (Exception ex)
            {
                Log.Error(ex, $"I am from {ControllerContext.ActionDescriptor.ControllerName} {ControllerContext.ActionDescriptor.MethodInfo.Name}... ");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
