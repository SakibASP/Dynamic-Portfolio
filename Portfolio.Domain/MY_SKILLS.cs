using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portfolio.Domain;

[Table("MY_SKILLS")]
public class MY_SKILLS
{
    [Key]
    public int AUTO_ID { get; set; }
    public string? SKILL_NAME { get; set; }
    public int? SKILL_PERCENTAGE { get; set; }
}
