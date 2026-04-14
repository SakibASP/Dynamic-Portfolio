using Microsoft.AspNetCore.Mvc.Filters;
using Portfolio.Application.Abstractions;

namespace Portfolio.Web.Filters;

/// <summary>
/// Global action filter that records each visit. All the logic lives in
/// <see cref="IVisitorTrackingService"/> — this filter is a one-liner that just
/// dispatches to it. Scoped so EF services can be resolved per request.
/// </summary>
public class VisitorTrackingFilter : IAsyncActionFilter
{
    private readonly IVisitorTrackingService _tracker;

    public VisitorTrackingFilter(IVisitorTrackingService tracker)
    {
        _tracker = tracker;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var ip = context.HttpContext.Connection.RemoteIpAddress?.ToString();
        var ua = context.HttpContext.Request.Headers.UserAgent.ToString();

        // Fire-and-await on a best-effort basis; the service swallows and logs its own errors.
        await _tracker.TrackAsync(ip, ua, context.HttpContext.RequestAborted);
        await next();
    }
}
