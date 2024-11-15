namespace Portfolio.Utils
{
    public class SaveRequestModel<T>
    {
        // Specify the time zone for Bangladesh
        private static readonly TimeZoneInfo bdTimeZone = TimeZoneInfo.FindSystemTimeZoneById(Constant.bangladeshTimezone);
        public T? Item { get; set; }
        public IList<T>? Items { get; set; }
        public int? Id { get; set; }
        public string? UserName { get; set; }
        public DateTime BdCurrentTime { get; set; } = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, bdTimeZone);
    }
}
