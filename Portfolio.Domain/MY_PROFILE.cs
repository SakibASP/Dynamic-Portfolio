using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portfolio.Domain;

[Table("MY_PROFILE")]
public class MY_PROFILE
{
    [Key]
    public int AUTO_ID { get; set; }
    public string? MY_NAME { get; set; }
    public DateTime? DATE_OF_BIRTH { get; set; }
    public string? DESIGNATION { get; set; }
    public string? MY_WEBSITE { get; set; }
    public string? DEGREE { get; set; }
    public string? PHONE { get; set; }
    public string? EMAIL { get; set; }
    public string? CURRENT_CITY { get; set; }
    public string? HOMETOWN { get; set; }
    public string? PROFILE_IMAGE { get; set; }
    public string? DES_1 { get; set; }
    public string? DES_2 { get; set; }
    public string? DES_3 { get; set; }
    public string? STATUS { get; set; }
    public string? TECH_STACK { get; set; }
}
