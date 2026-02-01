using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Utils;
using Portfolio.ViewModels;
using Portfolio.Web.Common;
using Serilog;

namespace Portfolio.Web.Controllers;

[Authorize]
public class LogsController(IWebHostEnvironment hostingEnvironment) : BaseController
{
    private readonly IWebHostEnvironment _hostingEnvironment = hostingEnvironment;
    public IActionResult Index()
    {
        return RedirectToAction(nameof(BackupInfoLogs));
    }

    public IActionResult BackupErrorLogs()
    {
        // Get the path to the folder containing the files
        string folderPath = Path.Combine(_hostingEnvironment.ContentRootPath, "Logs\\ErrorLogs");
        // Check if the directory exists
        if (!Directory.Exists(folderPath))
        {
            // If the directory doesn't exist, create it
            Directory.CreateDirectory(folderPath);
        }
        // Get the list of files in the folder
        var files = Directory.GetFiles(folderPath);

        // Create a list to store file information
        var fileViewModels = new List<FileInfoViewModel>();

        // Iterate through each file and extract information
        foreach (var file in files)
        {
            var fileInfo = new FileInfo(file);

            // Create a new FileViewModel object and populate it with file information
            var fileViewModel = new FileInfoViewModel
            {
                Name = Path.GetFileNameWithoutExtension(fileInfo.Name),
                ModifiedDate = fileInfo.LastWriteTime,
                Type = Path.GetExtension(fileInfo.FullName),
                Size = Math.Round(ConvertBytesToMegabytes(fileInfo.Length), 4),
                FilePath = fileInfo.FullName
            };

            // Add the FileViewModel to the list
            fileViewModels.Add(fileViewModel);
        }

        if (fileViewModels.Count > 0)
            fileViewModels = fileViewModels.OrderByDescending(x => x.ModifiedDate).ToList();

        // Pass the list of FileViewModels to the view
        return View(fileViewModels);
    }

    public IActionResult BackupInfoLogs()
    {
        // Get the path to the folder containing the files
        string folderPath = Path.Combine(_hostingEnvironment.ContentRootPath, "Logs\\InfoLogs");
        // Check if the directory exists
        if (!Directory.Exists(folderPath))
        {
            // If the directory doesn't exist, create it
            Directory.CreateDirectory(folderPath);
        }
        // Get the list of files in the folder
        var files = Directory.GetFiles(folderPath);

        // Create a list to store file information
        var fileViewModels = new List<FileInfoViewModel>();

        // Iterate through each file and extract information
        foreach (var file in files)
        {
            var fileInfo = new FileInfo(file);

            // Create a new FileViewModel object and populate it with file information
            var fileViewModel = new FileInfoViewModel
            {
                Name = Path.GetFileNameWithoutExtension(fileInfo.Name),
                ModifiedDate = fileInfo.LastWriteTime,
                Type = Path.GetExtension(fileInfo.FullName),
                Size = Math.Round(ConvertBytesToMegabytes(fileInfo.Length), 4),
                FilePath = fileInfo.FullName
            };

            // Add the FileViewModel to the list
            fileViewModels.Add(fileViewModel);
        }

        if (fileViewModels.Count > 0)
            fileViewModels = fileViewModels.OrderByDescending(x => x.ModifiedDate).ToList();

        // Pass the list of FileViewModels to the view
        return View(fileViewModels);
    }

    public IActionResult BackupFatalLogs()
    {
        // Get the path to the folder containing the files
        string folderPath = Path.Combine(_hostingEnvironment.ContentRootPath, "Logs\\FatalLogs");
        // Check if the directory exists
        if (!Directory.Exists(folderPath))
        {
            // If the directory doesn't exist, create it
            Directory.CreateDirectory(folderPath);
        }
        // Get the list of files in the folder
        var files = Directory.GetFiles(folderPath);

        // Create a list to store file information
        var fileViewModels = new List<FileInfoViewModel>();

        // Iterate through each file and extract information
        foreach (var file in files)
        {
            var fileInfo = new FileInfo(file);

            // Create a new FileViewModel object and populate it with file information
            var fileViewModel = new FileInfoViewModel
            {
                Name = Path.GetFileNameWithoutExtension(fileInfo.Name),
                ModifiedDate = fileInfo.LastWriteTime,
                Type = Path.GetExtension(fileInfo.FullName),
                Size = Math.Round(ConvertBytesToMegabytes(fileInfo.Length), 4),
                FilePath = fileInfo.FullName
            };

            // Add the FileViewModel to the list
            fileViewModels.Add(fileViewModel);
        }

        if (fileViewModels.Count > 0)
            fileViewModels = fileViewModels.OrderByDescending(x => x.ModifiedDate).ToList();

        // Pass the list of FileViewModels to the view
        return View(fileViewModels);
    }

    public IActionResult ClearBackupFolder(string folderName)
    {
        string returnActionName = "Backup" + folderName;
        try
        {
            const string rootLog = "Logs";
            //Backup Folder check or create
            string folderPath = string.Empty;

            folderPath = Path.Combine(_hostingEnvironment.ContentRootPath, rootLog, folderName);

            // Create a DirectoryInfo object for the specified folder
            DirectoryInfo directory = new(folderPath);

            // Check if the folder exists
            if (directory.Exists)
            {
                // Get the list of files in the folder
                FileInfo[] files = directory.GetFiles();

                // Delete each file in the folder
                foreach (FileInfo file in files)
                {
                    if (file.LastAccessTime.Date != BdCurrentTime.Date)
                        file.Delete();
                }
            }
            TempData[Constant.Success] = "Folder is cleared";
        }
        catch (Exception ex)
        {
            TempData[Constant.Error] = Constant.ErrorMessage;
            Log.Error(ex, $"I am from {ControllerContext.ActionDescriptor.ControllerName} {ControllerContext.ActionDescriptor.MethodInfo.Name}... {User.Identity?.Name}");

        }

        return RedirectToAction(returnActionName);
    }

    public IActionResult DownloadErrorTextFile(string filePath, string fileName)
    {
        string returnAction = string.Empty;
        if (filePath.Contains("InfoLogs"))
        {
            returnAction = "BackupInfoLogs";
        }
        else if (filePath.Contains("ErrorLogs"))
        {
            returnAction = "BackupErrorLogs";
        }
        else
        {
            returnAction = "BackupFatalLogs";
        }

        try
        {

            // Check if the file exists
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound(); // Return 404 Not Found if the file doesn't exist
            }

            // Get the MIME type of the file
            var contentType = "text/plain";

            // Set the Content-Disposition header to force the browser to download the file
            var contentDisposition = new System.Net.Mime.ContentDisposition
            {
                FileName = fileName + ".txt",
                Inline = false // Set to true if you want the browser to try to display the file inline (if supported)
            };

            Response.Headers.Append("Content-Disposition", contentDisposition.ToString());
            // Return the file
            return PhysicalFile(filePath, contentType);
        }
        catch (Exception ex)
        {
            TempData[Constant.Error] = Constant.ErrorMessage;
            Log.Error(ex, $"I am from {ControllerContext.ActionDescriptor.ControllerName} {ControllerContext.ActionDescriptor.MethodInfo.Name}... {User.Identity?.Name}");

        }
        return RedirectToAction(returnAction);
    }

    //Deleting the text File
    public IActionResult DeleteErrorTextFile(string filePath, string fileName)
    {
        string returnAction = string.Empty;
        if (filePath.Contains("InfoLogs"))
        {
            returnAction = "BackupInfoLogs";
        }
        else if (filePath.Contains("ErrorLogs"))
        {
            returnAction = "BackupErrorLogs";
        }
        else
        {
            returnAction = "BackupFatalLogs";
        }
        try
        {
            // Check if the file exists
            FileInfo file = new(filePath);
            if (file.Exists)
            {
                // Delete the file after it has been served to the client
                if (file.LastAccessTime.Date != BdCurrentTime.Date)
                {
                    file.Delete();
                    TempData[Constant.Success] = Constant.SuccessRmvMsg;
                }
                else
                {
                    TempData[Constant.Error] = "Sorry! Can not remove current log!";
                }
            }
        }
        catch (Exception ex)
        {
            TempData[Constant.Error] = Constant.ErrorMessage;
            Log.Error(ex, $"I am from {ControllerContext.ActionDescriptor.ControllerName} {ControllerContext.ActionDescriptor.MethodInfo.Name}... {User.Identity?.Name}");

        }
        return RedirectToAction(returnAction);
    }

    private static double ConvertBytesToMegabytes(long bytes)
    {
        return bytes / 1024f / 1024f;
    }

}
