using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Portfolio.Application.Abstractions;
using Portfolio.Infrastructure.Persistence;
using Portfolio.Application.Common;

namespace Portfolio.Infrastructure.Adapters;

/// <summary>
/// Records first-of-day visits per IP+OS+UA into the Visitors table, enriching with
/// an ip-api.com lookup. Replaces the ~90-line god-filter previously living in
/// Portfolio.Web.Common.BaseController.
/// </summary>
public class VisitorTrackingService : IVisitorTrackingService
{
    private static readonly TimeZoneInfo BdTimeZone =
        TimeZoneInfo.FindSystemTimeZoneById(Constant.bangladeshTimezone);

    private readonly PortfolioDbContext _context;
    private readonly IGeoLocationClient _geo;
    private readonly ILogger<VisitorTrackingService> _logger;

    public VisitorTrackingService(
        PortfolioDbContext context,
        IGeoLocationClient geo,
        ILogger<VisitorTrackingService> logger)
    {
        _context = context;
        _geo = geo;
        _logger = logger;
    }

    public async Task TrackAsync(string? ipAddress, string? userAgent, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(userAgent)) return;

        try
        {
            var visitor = UserAgentParser.Parse(userAgent);
            var bdNow = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BdTimeZone);

            var alreadySeen = await _context.Visitors
                .AsNoTracking()
                .AnyAsync(v => v.IPAddress == ipAddress
                            && v.VisitTime.Date == bdNow.Date
                            && v.OperatingSystem == visitor.OperatingSystem
                            && v.UserAgent == userAgent, cancellationToken);

            if (alreadySeen) return;

            var geo = await _geo.LookupAsync(ipAddress, cancellationToken);
            if (geo is not null)
            {
                visitor.City = geo.City;
                visitor.Country = geo.Country;
                visitor.Zip = geo.Zip;
                visitor.Timezone = geo.Timezone;
                visitor.Isp = geo.Isp;
                visitor.Org = geo.Org;
                visitor.As = geo.As;
            }

            visitor.IPAddress = ipAddress;
            visitor.UserAgent = userAgent;
            visitor.VisitTime = bdNow;

            await _context.Visitors.AddAsync(visitor, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Visitor tracking failed");
        }
    }
}
