using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portfolio.Domain;

[Table("EXPERIENCE")]
public class EXPERIENCE
{
    [Key]
    public int AUTO_ID { get; set; }
    public string? DESIGNATION { get; set; }
    public string? FROM_DATE { get; set; }
    public string? TO_DATE { get; set; }
    public string? INSTITUTE { get; set; }
    public int? SORT_ORDER { get; set; }
    public string? LOGO { get; set; }
    public bool IS_CURRENT { get; set; }
    public string? COMPANY_URL { get; set; }
}
