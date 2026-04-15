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

    public static readonly string[] All = [Heading, Text, Code, Image, Quote];
}
