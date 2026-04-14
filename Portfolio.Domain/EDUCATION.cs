using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portfolio.Domain;

[Table("EDUCATION")]
public class EDUCATION
{
    [Key]
    public int AUTO_ID { get; set; }
    public string? COURSE { get; set; }
    public string? FROM_DATE { get; set; }
    public string? TO_DATE { get; set; }
    public string? INSTITUTE { get; set; }
    public string? DESCRIPTION { get; set; }
}
