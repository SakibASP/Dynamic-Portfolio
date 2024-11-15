using Microsoft.Extensions.Hosting;
using Portfolio.Models;
using System.Net;

namespace Portfolio.Web.Common
{
    public static class Utility
    {
        public static async Task<byte[]?> Getimage(byte[]? img, IFormFileCollection files)
        {
            PROJECTS project = new();
            MemoryStream ms = new();
            if (files != null)
            {
                foreach (var file in files)
                {
                    await file.CopyToAsync(ms);
                    project.LOGO = ms.ToArray();

                    ms.Close();
                    await ms.DisposeAsync();

                    img = project.LOGO;
                }
            }
            return img;
        }

        public static string GetFilePathOfCV(IWebHostEnvironment _hostEnvironment)
        {
            string folderPath = Path.Combine(_hostEnvironment.WebRootPath, "CV");
            DirectoryInfo directory = new(folderPath);
            var fullName = directory.GetFiles().OrderByDescending(x => x.LastWriteTime).FirstOrDefault()?.FullName ?? "";
            return fullName;
        }

        public static async Task SaveFileAsync(string filePath, IFormFile file)
        {
            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);
        }

        public static string GetIpDetailsUrl(string? ipAddress)
        {
            var _url = $"http://ip-api.com/json/{ipAddress}?fields=city,country,zip,timezone,isp,org,as";
            return _url;
        }
    }
}
