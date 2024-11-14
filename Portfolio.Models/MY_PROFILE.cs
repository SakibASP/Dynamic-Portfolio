using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portfolio.Models
{
    [Table("MY_PROFILE")]
    public class MY_PROFILE
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int AUTO_ID { get; set; }

        [DisplayName("Name")]
        public string? MY_NAME { get; set; }

        [DisplayName("Birth Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DATE_OF_BIRTH { get; set; }

        [DisplayName("Designation")]
        public string? DESIGNATION { get; set;}
        [DisplayName("Age")]
        public int AGE { get; set;}

        [DisplayName("Website")]
        public string? MY_WEBSITE { get; set; }
        [DisplayName("Degree")]
        public string? DEGREE { get; set; }
        [DisplayName("Phone No.")]
        public string? PHONE { get; set; }
        [DisplayName("E-mail")]
        public string? EMAIL { get; set; }
        [DisplayName("City")]
        public string? CURRENT_CITY { get; set; }
        [DisplayName("Home Town")]
        public string? HOMETOWN { get; set; }

        [DisplayName("Image")]
        public string? PROFILE_IMAGE { get; set; }

        [DisplayName("Description 1")]
        [DataType(DataType.MultilineText)]
        public string? DES_1 { get; set; }

        [DisplayName("Description 2")]
        [DataType(DataType.MultilineText)]
        public string? DES_2 { get; set; }

        [DisplayName("Description 3")]
        [DataType(DataType.MultilineText)]
        public string? DES_3 { get; set; }
    }
}
