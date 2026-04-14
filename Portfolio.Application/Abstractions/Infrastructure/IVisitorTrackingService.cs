namespace Portfolio.Application.Abstractions;

/// <summary>
/// Application-layer contract for recording a visit. The concrete implementation
/// (persistence + IP geolocation) lives in the Infrastructure layer.
/// </summary>
public interface IVisitorTrackingService
{
    Task TrackAsync(string? ipAddress, string? userAgent, CancellationToken cancellationToken = default);
}
