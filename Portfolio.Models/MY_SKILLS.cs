using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portfolio.Models
{
    [Table("MY_SKILLS")]
    public class MY_SKILLS
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int AUTO_ID { get; set; }
        [DisplayName("Name")]
        public string? SKILL_NAME { get; set; }
        [DisplayName("Percentage")]
        public int? SKILL_PERCENTAGE { get; set; }
    }
}
