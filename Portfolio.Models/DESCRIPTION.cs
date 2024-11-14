using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Portfolio.Models;

namespace Portfolio.Models
{
    [Table("DESCRIPTION")]
    public class DESCRIPTION
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int AUTO_ID { get; set; }

        [DisplayName("Description")]
        public string? DESCRIPTION_TEXT { get; set; }
        [DisplayName("Type")]
        public int? TYPE_ID { get; set; }
        [DisplayName("Project")]
        public int? PROJECT_ID { get; set; }
        [DisplayName("Created By")]
        public string? CREATED_BY { get; set; }
        [DisplayName("Created Date")]
        public DateTime? CREATED_DATE { get; set; }
        [DisplayName("Modified By")]
        public string? MODIFIED_BY { get; set; }
        [DisplayName("Modified Date")]
        public DateTime? MODIFIED_DATE { get; set; }
        [DisplayName("Order")]
        public int? SORT_ORDER { get; set; }
        [DisplayName("Experience")]
        public int? EXPERIENCE_ID { get; set; }
        [ForeignKey(nameof(TYPE_ID))]
        public virtual DESCRIPTION_TYPE? DESCRIPTION_TYPE_ { get; set; }
        [ForeignKey(nameof(PROJECT_ID))]
        public virtual PROJECTS? PROJECT_ { get; set; }
        [ForeignKey(nameof(EXPERIENCE_ID))]
        public virtual EXPERIENCE? EXPERIENCE_ { get; set; }
    }
}
