namespace Portfolio.Web.Common;

/// <summary>
/// Presentation-layer file helpers. Visitor/geo helpers have been moved to
/// Portfolio.Infrastructure.Adapters (Infrastructure).
/// </summary>
public static class Utility
{
    public static async Task<byte[]?> GetImageBytes(byte[]? img, IFormFileCollection files)
    {
        if (files is null) return img;
        foreach (var file in files)
        {
            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            img = ms.ToArray();
        }
        return img;
    }

    public static string GetFilePathOfCV(IWebHostEnvironment hostEnvironment)
    {
        var folderPath = Path.Combine(hostEnvironment.WebRootPath, "CV");
        var directory = new DirectoryInfo(folderPath);
        return directory.GetFiles().OrderByDescending(x => x.LastWriteTime).FirstOrDefault()?.FullName ?? "";
    }

    public static async Task SaveFileAsync(string filePath, IFormFile file)
    {
        using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);
    }
}
