namespace Portfolio.ViewModels
{
    public class VisitorsViewModel
    {
		public string? IPAddress { get; set; }
		public string? OperatingSystem { get; set; }
		public string? OperatingSystemVersion { get; set; }
		public string? Browser { get; set; }
		public string? BrowserVersion { get; set; }
		public string? DeviceType { get; set; }
		public string? DeviceBrand { get; set; }
		public string? DeviceModel { get; set; }
		public string? City { get; set; }
		public string? Country { get; set; }
		public string? Timezone { get; set; }
		public DateTime VisitTime { get; set; }
		public int TotalRows { get; set; } = 0;
    }
}
