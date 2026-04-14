using Portfolio.Domain;
using UAParser;

namespace Portfolio.Infrastructure.Adapters;

/// <summary>
/// Translates a raw User-Agent header into a <see cref="Visitors"/> device descriptor.
/// Kept internal to Infrastructure because it depends on UAParser.
/// </summary>
public static class UserAgentParser
{
    public static Visitors Parse(string userAgent)
    {
        var uaParser = Parser.GetDefault();
        var clientInfo = uaParser.Parse(userAgent ?? string.Empty);

        var os = string.IsNullOrEmpty(clientInfo?.OS.Family) || clientInfo?.OS.Family == "Other"
            ? GuessOs(userAgent ?? string.Empty)
            : clientInfo!.OS.Family;

        return new Visitors
        {
            Browser = clientInfo?.UserAgent.Family,
            BrowserVersion = clientInfo?.UserAgent.Major,
            OperatingSystem = os,
            OperatingSystemVersion = clientInfo?.OS.Major,
            DeviceType = clientInfo?.Device.Family,
            DeviceBrand = clientInfo?.Device?.Brand,
            DeviceModel = clientInfo?.Device?.Model
        };
    }

    private static string GuessOs(string ua)
    {
        if (ua.Contains("Android")) return "Android";
        if (ua.Contains("Windows")) return "Windows";
        if (ua.Contains("Mac OS")) return "macOS";
        if (ua.Contains("Linux")) return "Linux";
        if (ua.Contains("Google-Safety") || ua == "Google") return "Google";
        if (ua.Contains("facebookexternalhit")) return "Facebook";
        return "Unknown";
    }
}
