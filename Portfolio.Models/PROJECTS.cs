using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portfolio.Models
{
    [Table("PROJECTS")]
    public class PROJECTS
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int AUTO_ID { get; set; }
        [DisplayName("Project Name")]
        public string? PROJECT_NAME { get; set; }

        [DisplayName("Image")]
        public byte[]? LOGO { get; set; }
        
    }
}
