using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portfolio.Models;

[Table("EDUCATION")]
public class EDUCATION
{
    [Key]
    [HiddenInput(DisplayValue = false)]
    public int AUTO_ID { get; set; }
    [DisplayName("Course")]
    public string? COURSE { get; set; }
    [DisplayName("From Date")]
    public string? FROM_DATE { get; set; }
    [DisplayName("To Date")]
    public string? TO_DATE { get; set; }
    [DisplayName("Institute")]
    public string? INSTITUTE { get; set; }
    [DisplayName("Description")]
    [DataType(DataType.MultilineText)]
    public string? DESCRIPTION { get; set; }
}
