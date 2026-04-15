using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portfolio.Domain;

[Table("BLOG_POST_BLOCKS")]
public class BLOG_POST_BLOCKS
{
    [Key]
    public int AUTO_ID { get; set; }

    public int BLOG_POST_ID { get; set; }

    [Required, StringLength(32)]
    public string BLOCK_TYPE { get; set; } = "Text";

    public string? CONTENT { get; set; }

    [StringLength(32)] public string? CODE_LANGUAGE { get; set; }
    [StringLength(64)] public string? FONT_FAMILY { get; set; }
    [StringLength(16)] public string? FONT_SIZE { get; set; }
    [StringLength(32)] public string? FONT_STYLE { get; set; }
    [StringLength(16)] public string? TEXT_ALIGN { get; set; }

    [StringLength(512)] public string? IMAGE_PATH { get; set; }
    [StringLength(256)] public string? IMAGE_CAPTION { get; set; }

    public int SORT_ORDER { get; set; }

    [StringLength(256)] public string? CREATED_BY { get; set; }
    public DateTime CREATED_DATE { get; set; }
    [StringLength(256)] public string? UPDATED_BY { get; set; }
    public DateTime? UPDATED_DATE { get; set; }
}

public static class BlogBlockType
{
    public const string Heading = "Heading";
    public const string Text    = "Text";
    public const string Code    = "Code";
    public const string Image   = "Image";
    public const string Quote   = "Quote";
    public const string Video   = "Video";

    public static readonly string[] All = [Heading, Text, Code, Image, Quote, Video];

    /// <summary>
    /// Extracts a YouTube video id from any common URL form
    /// (watch?v=, youtu.be/, shorts/, embed/) or returns the input unchanged
    /// if it's already a bare 11-char id. Returns null for anything unrecognised.
    /// </summary>
    public static string? ExtractYouTubeId(string? urlOrId)
    {
        if (string.IsNullOrWhiteSpace(urlOrId)) return null;
        var s = urlOrId.Trim();

        // Bare id (11 chars, YouTube-safe charset)
        if (s.Length == 11 && System.Text.RegularExpressions.Regex.IsMatch(s, "^[A-Za-z0-9_-]{11}$"))
            return s;

        var patterns = new[]
        {
            @"(?:youtube\.com/watch\?v=)([A-Za-z0-9_-]{11})",
            @"(?:youtu\.be/)([A-Za-z0-9_-]{11})",
            @"(?:youtube\.com/embed/)([A-Za-z0-9_-]{11})",
            @"(?:youtube\.com/shorts/)([A-Za-z0-9_-]{11})",
            @"(?:youtube\.com/v/)([A-Za-z0-9_-]{11})"
        };
        foreach (var p in patterns)
        {
            var m = System.Text.RegularExpressions.Regex.Match(s, p);
            if (m.Success) return m.Groups[1].Value;
        }
        return null;
    }
}
