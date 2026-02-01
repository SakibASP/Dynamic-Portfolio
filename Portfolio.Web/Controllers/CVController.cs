using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.ViewModels;
using Portfolio.Web.Common;

namespace Portfolio.Web.Controllers;

public class CVController(IWebHostEnvironment hostEnvironment) : BaseController
{
    private readonly IWebHostEnvironment _hostEnvironment = hostEnvironment;

    [Authorize]
    public IActionResult Index()
    {
        string filePath = Path.Combine(_hostEnvironment.WebRootPath, "CV");
        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }

        string[] files = Directory.GetFiles(filePath);

        List<FileInfoViewModel> fileModel = [];

        foreach (string file in files)
        {
            FileInfo? fileInfo = new(file);
            FileInfoViewModel fileInfoModel = new()
            {
                Name = Path.GetFileNameWithoutExtension(fileInfo.Name),
                ModifiedDate = fileInfo.LastWriteTime,
                Type = Path.GetExtension(fileInfo.FullName),
                Size = Math.Round(ConvertBytesToMegabytes(fileInfo.Length),3),
                FilePath = fileInfo.FullName,
            };
            fileModel.Add(fileInfoModel);
        }

        if (fileModel.Count > 0)
        {
            fileModel = [.. fileModel.OrderByDescending(x => x.ModifiedDate)];
        }

        return View(fileModel);
    }

    public IActionResult DownloadCV(string filePath)
    {
        try
        {
            if (System.IO.File.Exists(filePath))
            {
                string fileName = Path.GetFileName(filePath);
                return PhysicalFile(filePath, "application/octet-stream", fileName);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        catch { }
        
        return RedirectToAction(nameof(Index));
    }

    [Authorize]
    public IActionResult RemoveCV(string filePath)
    {
        try
        {
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            else
            {
                TempData["State"] = "File Not Found";
            }
        }
        catch { }

        return RedirectToAction(nameof(Index));
    }

    [Authorize]
    public IActionResult ClearCVFolder()
    {
        try {
            string folderPath = Path.Combine(_hostEnvironment.WebRootPath, "CV");
            DirectoryInfo directory = new(folderPath);

            if (directory.Exists)
            {
                FileInfo[] files = directory.GetFiles();
                foreach (FileInfo file in files)
                {
                    file.Delete();
                }
            }
        }
        catch { }
        return RedirectToAction(nameof(Index));
    }

    [Authorize]
    public IActionResult UploadCV()
    {
        return View();
    }

    [Authorize]
    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        try
        {
            string fileDirectory = Path.Combine(_hostEnvironment.WebRootPath, "CV");
            if (!Directory.Exists(fileDirectory))
            {
                Directory.CreateDirectory(fileDirectory);
            }

            // Generate a unique file name (e.g., using Guid or timestamp)
            var fileName = file.FileName.Replace(" ","");
            var filePath = Path.Combine(fileDirectory, fileName);

            // Save the file to the specified location
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok($"File uploaded successfully. Saved as {fileName}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error uploading file: {ex.Message}");
        }
    }

    public IActionResult CV_Viewer()
    {
        var filePath = Utility.GetFilePathOfCV(_hostEnvironment);
        ViewBag.FilePath = filePath;
        return View();
    }

    public IActionResult ShowCV(string filePath)
    {
        // Check if the file exists
        if (!System.IO.File.Exists(filePath))
        {
            return NotFound();
        }

        // Return the PDF file
        var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        return new FileStreamResult(fileStream, "application/pdf");
    }

    private static double ConvertBytesToMegabytes(long bytes)
    {
        return (bytes / 1024f) / 1024f;
    }
}
