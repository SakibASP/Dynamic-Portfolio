using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portfolio.Domain;

[Table("PROFILE_COVER")]
public class PROFILE_COVER
{
    [Key]
    public int AUTO_ID { get; set; }
    public string COVER_NAME { get; set; } = default!;
    public string COVER_DESC { get; set; } = default!;
    public string? COVER_IMAGE { get; set; }
}
