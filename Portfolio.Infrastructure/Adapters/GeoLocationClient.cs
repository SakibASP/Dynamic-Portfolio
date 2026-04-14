using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Portfolio.Application.Abstractions;
using Portfolio.Application.DTOs;

namespace Portfolio.Infrastructure.Adapters;

/// <summary>
/// ip-api.com adapter. Uses <see cref="IHttpClientFactory"/> so timeouts, retries and
/// sockets are managed by the host.
/// </summary>
public class GeoLocationClient : IGeoLocationClient
{
    public const string HttpClientName = "ip-api";

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<GeoLocationClient> _logger;

    public GeoLocationClient(IHttpClientFactory httpClientFactory, ILogger<GeoLocationClient> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<GeoLocation?> LookupAsync(string? ipAddress, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(ipAddress))
            return null;

        try
        {
            var client = _httpClientFactory.CreateClient(HttpClientName);
            var url = $"http://ip-api.com/json/{ipAddress}?fields=city,country,zip,timezone,isp,org,as";
            var response = await client.GetAsync(url, cancellationToken);
            if (!response.IsSuccessStatusCode) return null;

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            dynamic? result = JsonConvert.DeserializeObject(content);
            if (result is null) return null;

            return new GeoLocation
            {
                City = result.city,
                Country = result.country,
                Zip = result.zip,
                Timezone = result.timezone,
                Isp = result.isp,
                Org = result.org,
                As = result["as"]
            };
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Geo lookup failed for {Ip}", ipAddress);
            return null;
        }
    }
}
