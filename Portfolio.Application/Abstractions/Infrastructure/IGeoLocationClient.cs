using Portfolio.Application.DTOs;

namespace Portfolio.Application.Abstractions;

/// <summary>
/// External adapter for IP → location lookup (implemented against ip-api.com).
/// Lives behind an interface so the app layer / visitor-tracking service does not
/// depend on HttpClient or a particular provider.
/// </summary>
public interface IGeoLocationClient
{
    Task<GeoLocation?> LookupAsync(string? ipAddress, CancellationToken cancellationToken = default);
}
