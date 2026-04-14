using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Application.Common;

namespace Portfolio.Web.Common;

/// <summary>
/// Thin base for MVC controllers. Only exposes the current user + a Bangladesh-time
/// clock helper. Visitor tracking has been moved to <see cref="Filters.VisitorTrackingFilter"/>
/// which calls into the IVisitorTrackingService (Infrastructure).
/// </summary>
public abstract class BaseController : Controller
{
    private static readonly TimeZoneInfo BdTimeZone =
        TimeZoneInfo.FindSystemTimeZoneById(Constant.bangladeshTimezone);

    protected string? CurrentUserId =>
        User.FindFirstValue(ClaimTypes.NameIdentifier);

    protected string? CurrentUserName =>
        User.Identity?.IsAuthenticated == true ? User.Identity.Name : null;

    protected static DateTime BdCurrentTime =>
        TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BdTimeZone);
}
