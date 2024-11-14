using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portfolio.Models
{
    [Table("PROFILE_COVER")]
    public class PROFILE_COVER
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int AUTO_ID { get; set; }
        [DisplayName("Name")]
        public string COVER_NAME { get; set; }
        [DisplayName("Description")]
        public string COVER_DESC { get; set; }

        [DisplayName("Cover Picture")]
        public string? COVER_IMAGE { get; set; }
    }
}
