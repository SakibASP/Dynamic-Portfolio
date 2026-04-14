using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portfolio.Domain;

[Table("PROJECTS")]
public class PROJECTS
{
    [Key]
    public int AUTO_ID { get; set; }
    public string? PROJECT_NAME { get; set; }
    public byte[]? LOGO { get; set; }
}
