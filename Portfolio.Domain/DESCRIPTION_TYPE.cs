using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portfolio.Domain;

[Table("DESCRIPTION_TYPE")]
public class DESCRIPTION_TYPE
{
    [Key]
    public int AUTO_ID { get; set; }
    public string? TYPE { get; set; }
}
