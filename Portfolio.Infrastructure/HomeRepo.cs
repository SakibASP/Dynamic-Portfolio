using Microsoft.EntityFrameworkCore;
using Portfolio.Application.Abstractions;
using Portfolio.Infrastructure.Adapters;
using Portfolio.Infrastructure.Persistence;
using Portfolio.Application.DTOs;

namespace Portfolio.Infrastructure;

public class HomeRepo(PortfolioDbContext context) : IHomeRepo
{
    private readonly PortfolioDbContext _context = context;

    public async Task<int?> GetUnreadMessagesCountAsync()
    {
        return await _context.CONTACTS
            .Where(x => x.IsConfirmed == null || x.IsConfirmed == 0)
            .AsNoTracking()
            .CountAsync();
    }

    public async Task<bool> SaveLocationAsync(LocationRequest request)
    {
        if (request is null) return false;

        // Resolve the OS from the raw UA here so callers (controllers) don't have to
        // depend on a specific UA-parsing library — Web just forwards the raw UA.
        var os = UserAgentParser.Parse(request.UserAgent).OperatingSystem;

        var visitor = await _context.Visitors
            .FirstOrDefaultAsync(v => v.IPAddress == request.IPAddress
                                   && v.VisitTime.Date == request.VisitTime.Date
                                   && v.OperatingSystem == os
                                   && v.UserAgent == request.UserAgent);

        if (visitor is null) return false;

        visitor.Latitude = request.Latitude;
        visitor.Longitude = request.Longitude;
        await _context.SaveChangesAsync();
        return true;
    }
}
