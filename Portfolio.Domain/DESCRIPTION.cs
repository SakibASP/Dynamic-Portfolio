using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portfolio.Domain;

[Table("DESCRIPTION")]
public class DESCRIPTION
{
    [Key]
    public int AUTO_ID { get; set; }
    public string? DESCRIPTION_TEXT { get; set; }
    public int? TYPE_ID { get; set; }
    public int? PROJECT_ID { get; set; }
    public string? CREATED_BY { get; set; }
    public DateTime? CREATED_DATE { get; set; }
    public string? MODIFIED_BY { get; set; }
    public DateTime? MODIFIED_DATE { get; set; }
    public int? SORT_ORDER { get; set; }
    public int? EXPERIENCE_ID { get; set; }

    [ForeignKey(nameof(TYPE_ID))]
    public virtual DESCRIPTION_TYPE? DESCRIPTION_TYPE_ { get; set; }

    [ForeignKey(nameof(PROJECT_ID))]
    public virtual PROJECTS? PROJECT_ { get; set; }

    [ForeignKey(nameof(EXPERIENCE_ID))]
    public virtual EXPERIENCE? EXPERIENCE_ { get; set; }
}
