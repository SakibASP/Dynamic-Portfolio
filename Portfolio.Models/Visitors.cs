using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portfolio.Models;

[Table(nameof(Visitors))]
public class Visitors
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int AutoId { get; set; }
    public string? IPAddress { get; set; }
    public string? UserAgent { get; set; } 
    public string? Browser { get; set; } // Example: Chrome, Firefox, etc.
    public string? BrowserVersion { get; set; }
    public string? OperatingSystem { get; set; } // Example: Windows, macOS, Linux, etc.
    public string? OperatingSystemVersion { get; set; }
    public string? DeviceType { get; set; }
    public string? DeviceBrand { get; set; }
    public string? DeviceModel { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? Zip { get; set; }
    public string? Timezone { get; set; }
    public string? Isp { get; set; }
    public string? Org { get; set; }
    public string? As { get; set; }
    public DateTime VisitTime { get; set; } = DateTime.Now;

    public string? Latitude { get; set; }
    public string? Longitude { get; set; }
}
