using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portfolio.Domain
{
    [Table("CONTACTS")]
    public class CONTACTS
    {
        [Key]
        public int AUTO_ID { get; set; }
        public int? IsConfirmed { get; set; }
        public string? NAME { get; set; }
        public string? SUBJECT { get; set; }
        public string? MESSAGE { get; set; }
        public string? EMAIL { get; set; }
        public string? PHONE { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public DateTime? MODIFIED_DATE { get; set; }
        public string? LATITUDE { get; set; }
        public string? LONGITUTE { get; set; }
    }
}
