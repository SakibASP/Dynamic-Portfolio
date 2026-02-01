namespace Portfolio.ViewModels;

public class LocationRequest
{
    public string Latitude { get; set; } = default!;
    public string Longitude { get; set; } = default!;   
    public string OperatingSystem { get; set; } = default!;   
    public string UserAgent { get; set; } = default!;   
    public string IPAddress { get; set; } = default!;   
    public DateTime VisitTime { get; set; } = default!;   
}
