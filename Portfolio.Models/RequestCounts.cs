using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portfolio.Models
{
    [Table(nameof(RequestCounts))]
    public class RequestCounts
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AutoId { get; set; }
        public int GetCount { get; set; }
        public int PostCount { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
