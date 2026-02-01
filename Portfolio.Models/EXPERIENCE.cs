using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portfolio.Models;

[Table("EXPERIENCE")]
public class EXPERIENCE
{
    [Key]
    [HiddenInput(DisplayValue = false)]
    public int AUTO_ID { get; set; }
    [DisplayName("Designation")]
    public string? DESIGNATION { get; set; }
    [DisplayName("From Date")]
    public string? FROM_DATE { get; set; }
    [DisplayName("To Date")]
    public string? TO_DATE { get; set; }
    [DisplayName("Institute")]
    public string? INSTITUTE { get; set; }
    [DisplayName("Sort Order")]
    public int? SORT_ORDER { get; set; }
}
