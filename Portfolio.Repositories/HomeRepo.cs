using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Portfolio.Interfaces;
using Portfolio.Repositories.Data;
using Portfolio.ViewModels;

namespace Portfolio.Repositories;

public class HomeRepo(PortfolioDbContext context) : IHomeRepo, IAsyncDisposable
{
    private readonly PortfolioDbContext _context = context;

    public async Task<int?> GetUnreadMessagesCountAsync()
    {
        return await _context.CONTACTS.Where(x => x.IsConfirmed == null || x.IsConfirmed == 0).AsNoTracking().CountAsync();
    }

    public async Task<bool> SaveLocationAsync(LocationRequest request)
    {
        if (request is null) return false;
        var visitor = await _context.Visitors
                            .AsNoTracking()
                            .FirstOrDefaultAsync(v => v.IPAddress == request.IPAddress
                                && v.VisitTime.Date == request.VisitTime.Date
                                && v.OperatingSystem == request.OperatingSystem
                                && v.UserAgent == request.UserAgent);

        if (visitor is  null) return false;

        visitor.Latitude = request.Latitude;
        visitor.Longitude = request.Longitude;
        _context.Visitors.Update(visitor);
        await _context.SaveChangesAsync();
        return true;
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}
