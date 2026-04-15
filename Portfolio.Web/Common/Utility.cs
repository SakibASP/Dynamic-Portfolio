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

    /// <summary>
    /// Saves the first uploaded file under wwwroot/&lt;subfolder&gt;/ and returns the absolute
    /// server path stored in the DB (matching the PROFILE_COVER.COVER_IMAGE convention).
    /// Returns null if no file was uploaded.
    /// </summary>
    public static async Task<string?> SaveUploadedAsync(IFormFile? file, IWebHostEnvironment env, params string[] subfolder)
    {
        if (file is null || file.Length == 0) return null;

        var directory = Path.Combine(new[] { env.WebRootPath }.Concat(subfolder).ToArray());
        if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

        var safeName = Path.GetFileName(file.FileName).Replace(" ", "_");
        var stamp    = DateTime.Now.ToString("yyyyMMddHHmmss");
        var full     = Path.Combine(directory, $"{stamp}_{safeName}");
        await SaveFileAsync(full, file);
        return full;
    }

    /// <summary>
    /// Deletes a file previously stored by <see cref="SaveUploadedAsync"/>. Safe to call
    /// with null/empty/nonexistent paths.
    /// </summary>
    public static void DeleteIfExists(string? absolutePath)
    {
        if (!string.IsNullOrWhiteSpace(absolutePath) && File.Exists(absolutePath))
            File.Delete(absolutePath);
    }

    /// <summary>
    /// Converts an absolute wwwroot-rooted path stored in the DB into a browser URL.
    /// </summary>
    public static string? ToWebUrl(string? absolutePath, string webRootPath)
    {
        if (string.IsNullOrWhiteSpace(absolutePath)) return null;
        if (absolutePath.StartsWith('/') || absolutePath.StartsWith("http", StringComparison.OrdinalIgnoreCase))
            return absolutePath;
        var rel = absolutePath.Replace(webRootPath, "", StringComparison.OrdinalIgnoreCase)
                              .Replace('\\', '/');
        if (!rel.StartsWith('/')) rel = "/" + rel;
        return rel;
    }
}
