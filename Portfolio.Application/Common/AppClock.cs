namespace Portfolio.Application.Common;

/// <summary>
/// Centralised clock for the Bangladesh timezone. Was previously duplicated as a
/// static field in BaseController, SaveRequestModel, VisitorTrackingService, etc.
/// Consolidated here so the Application layer owns the canonical "now".
/// </summary>
public static class AppClock
{
    private static readonly TimeZoneInfo BdTimeZone =
        TimeZoneInfo.FindSystemTimeZoneById(Constant.bangladeshTimezone);

    public static DateTime BdNow =>
        TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BdTimeZone);
}
