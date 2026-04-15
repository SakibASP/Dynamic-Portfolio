using Microsoft.EntityFrameworkCore;
using Portfolio.Application.Abstractions;
using Portfolio.Application.DTOs;
using Portfolio.Domain;
using Portfolio.Infrastructure.Adapters;
using Portfolio.Infrastructure.Persistence;

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

    public async Task<MY_PROFILE?> GetProfileAsync() =>
        await _context.MY_PROFILE.AsNoTracking().FirstOrDefaultAsync();

    public async Task<EXPERIENCE?> GetCurrentExperienceAsync()
    {
        // 1) Explicit IS_CURRENT flag wins.
        var flagged = await _context.EXPERIENCE.AsNoTracking()
            .Where(e => e.IS_CURRENT)
            .OrderByDescending(e => e.SORT_ORDER ?? 0)
            .ThenByDescending(e => e.AUTO_ID)
            .FirstOrDefaultAsync();
        if (flagged != null) return flagged;

        // 2) Otherwise, any row whose TO_DATE reads "Present" — pick the newest.
        var present = await _context.EXPERIENCE.AsNoTracking()
            .Where(e => e.TO_DATE != null && e.TO_DATE.ToLower().Contains("present"))
            .OrderByDescending(e => e.FROM_DATE)
            .ThenByDescending(e => e.AUTO_ID)
            .FirstOrDefaultAsync();
        if (present != null) return present;

        // 3) Ultimate fallback: most recently inserted row.
        return await _context.EXPERIENCE.AsNoTracking()
            .OrderByDescending(e => e.AUTO_ID)
            .FirstOrDefaultAsync();
    }

    public async Task<IList<EXPERIENCE>> GetCompaniesWithLogosAsync()
    {
        return await _context.EXPERIENCE.AsNoTracking()
            .Where(e => e.LOGO != null && e.LOGO != "")
            .OrderByDescending(e => e.IS_CURRENT)
            .ThenByDescending(e => e.SORT_ORDER)
            .ToListAsync();
    }

    public async Task<IList<BLOG_POSTS>> GetFeaturedBlogPostsAsync(int take)
    {
        return await _context.BLOG_POSTS.AsNoTracking()
            .Where(p => p.IS_PUBLISHED)
            .OrderByDescending(p => p.PUBLISHED_DATE ?? p.CREATED_DATE)
            .Take(take)
            .ToListAsync();
    }
}
