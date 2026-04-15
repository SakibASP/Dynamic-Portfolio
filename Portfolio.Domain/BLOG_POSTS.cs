using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portfolio.Domain;

[Table("BLOG_POSTS")]
public class BLOG_POSTS
{
    [Key]
    public int AUTO_ID { get; set; }

    [Required, StringLength(256)]
    public string TITLE { get; set; } = string.Empty;

    [StringLength(256)]
    public string? SLUG { get; set; }

    [StringLength(1000)]
    public string? SUMMARY { get; set; }

    [StringLength(512)]
    public string? COVER_IMAGE { get; set; }

    [StringLength(512)]
    public string? TAGS { get; set; }

    public bool IS_PUBLISHED { get; set; }
    public DateTime? PUBLISHED_DATE { get; set; }
    public int VIEW_COUNT { get; set; }

    [StringLength(256)] public string? CREATED_BY { get; set; }
    public DateTime CREATED_DATE { get; set; }
    [StringLength(256)] public string? UPDATED_BY { get; set; }
    public DateTime? UPDATED_DATE { get; set; }

    [NotMapped]
    public IList<BLOG_POST_BLOCKS> BLOCKS { get; set; } = new List<BLOG_POST_BLOCKS>();
}
