using Portfolio.Models;
using UAParser;

namespace Portfolio.Web.Common;

public static class Utility
{
    public static async Task<byte[]?> GetImageBytes(byte[]? img, IFormFileCollection files)
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
        return $"http://ip-api.com/json/{ipAddress}?fields=city,country,zip,timezone,isp,org,as";
    }

    public static Visitors GetVisitorsDeviceInfo(string userAgent)
    {
        var uaParser = Parser.GetDefault();
        ClientInfo? clientInfo = uaParser.Parse(userAgent);

        Visitors visitors = new()
        {
            Browser = clientInfo?.UserAgent.Family,
            BrowserVersion = clientInfo?.UserAgent.Major,
            OperatingSystem = string.IsNullOrEmpty(clientInfo?.OS.Family) || clientInfo?.OS.Family == "Other" ? GetOperatingSystem(userAgent) : clientInfo?.OS.Family,
            OperatingSystemVersion = clientInfo?.OS.Major,
            DeviceType = clientInfo?.Device.Family,
            DeviceBrand = clientInfo?.Device?.Brand,
            DeviceModel = clientInfo?.Device?.Model
        };
        return visitors;
    }

    private static string GetOperatingSystem(string userAgent)
    {
        // Logic to extract operating system from user agent string
        // You can use a library like UserAgentUtils or implement custom logic
        // For simplicity, let's assume a basic implementation
        if (userAgent.Contains("Android"))
        {
            // If "Android" is found in the User-Agent string, it's likely an Android device
            return "Android";
        }
        else if (userAgent.Contains("Windows"))
        {
            return "Windows";
        }
        else if (userAgent.Contains("Mac OS"))
        {
            return "macOS";
        }
        else if (userAgent.Contains("Linux"))
        {
            return "Linux";
        }
        else if (userAgent.Contains("Google-Safety") || userAgent.ToString() == "Google")
        {
            return "Google";
        }
        else if (userAgent.Contains("facebookexternalhit"))
        {
            return "Facebook";
        }
        else
        {
            return "Unknown";
        }
    }
}
