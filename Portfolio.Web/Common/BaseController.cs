using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Portfolio.Web.Data;
using Portfolio.Models;
using System.Security.Claims;
using UAParser;

namespace Portfolio.Web.Common
{
    public class BaseController : Controller 
    {
        private static readonly TimeZoneInfo bdTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Bangladesh Standard Time");
        public string? CurrentUserId { get; set; }
        public string? CurrentUserName { get; set; }
        public static DateTime BdCurrentTime { get; set; } = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, bdTimeZone);

        public override async Task OnActionExecutionAsync(ActionExecutingContext filterContext, ActionExecutionDelegate next)
        {
            using ApplicationDbContext _context = new();

            if (filterContext.HttpContext.User.Identity!.IsAuthenticated)
            {
                CurrentUserName = filterContext.HttpContext.User.Identity.Name;
                CurrentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            }

            try
            {
                var ipAddress = filterContext.HttpContext.Connection.RemoteIpAddress?.ToString();
                var userAgent = filterContext.HttpContext.Request.Headers.UserAgent.ToString();
                var userInfo = GetVisitorsDeviceInfo(userAgent);

                if (userInfo is not null)
                {
                    var existingVisitor = await _context.Visitors
                        .FirstOrDefaultAsync(v => v.IPAddress == ipAddress && v.VisitTime.Date == BdCurrentTime.Date && v.OperatingSystem == userInfo.OperatingSystem && v.UserAgent == userAgent);

                    if (existingVisitor is null)
                    {
                        Visitors visitor = userInfo;

                        // Make request to ip-api.com API
                        using var client = new HttpClient();
                        Dictionary<string, string> dictVisitor = [];

                        var _url = Utility.GetIpDetailsUrl(ipAddress);
                        var response = await client.GetAsync(_url);
                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(content)!;

                            if (result != null)
                            {
                                dictVisitor["City"] = result.city;
                                dictVisitor["Country"] = result.country;
                                dictVisitor["Zip"] = result.zip;
                                dictVisitor["Timezone"] = result.timezone;
                                dictVisitor["Isp"] = result.isp;
                                dictVisitor["Org"] = result.org;
                                dictVisitor["As"] = result["as"];
                            }
                        }

                        visitor.City = dictVisitor.GetValueOrDefault("City");
                        visitor.Country = dictVisitor.GetValueOrDefault("Country");
                        visitor.Zip = dictVisitor.GetValueOrDefault("Zip");
                        visitor.Timezone = dictVisitor.GetValueOrDefault("Timezone");
                        visitor.Isp = dictVisitor.GetValueOrDefault("Isp");
                        visitor.Org = dictVisitor.GetValueOrDefault("Org");
                        visitor.As = dictVisitor.GetValueOrDefault("As");
                        visitor.IPAddress = ipAddress;
                        visitor.UserAgent = userAgent;
                        visitor.VisitTime = BdCurrentTime;

                        await _context.Visitors.AddAsync(visitor);
                    }

                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                // Log exception here
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                // Call the next action in the pipeline
                await next();
            }
        }

        private static Visitors GetVisitorsDeviceInfo(string userAgent)
        {
            var uaParser = Parser.GetDefault();
            ClientInfo? clientInfo = uaParser.Parse(userAgent);

            Visitors visitors = new()
            {
                Browser = clientInfo?.UserAgent.Family,
                BrowserVersion = clientInfo?.UserAgent.Major,
                OperatingSystem = string.IsNullOrEmpty(clientInfo?.OS.Family) || clientInfo?.OS.Family == "Other" ? GetOperatingSystem(userAgent) : clientInfo?.OS.Family,
                OperatingSystemVersion = clientInfo?.OS.Major,
                DeviceType = clientInfo?.Device.Family,
                DeviceBrand = clientInfo?.Device?.Brand,
                DeviceModel = clientInfo?.Device?.Model
            };
            return visitors;
        }

        private static string GetOperatingSystem(string userAgent)
        {
            // Logic to extract operating system from user agent string
            // You can use a library like UserAgentUtils or implement custom logic
            // For simplicity, let's assume a basic implementation
            if (userAgent.Contains("Android"))
            {
                // If "Android" is found in the User-Agent string, it's likely an Android device
                return "Android";
            }
            else if (userAgent.Contains("Windows"))
            {
                return "Windows";
            }
            else if (userAgent.Contains("Mac OS"))
            {
                return "macOS";
            }
            else if (userAgent.Contains("Linux"))
            {
                return "Linux";
            }
            else if (userAgent.Contains("Google-Safety") || userAgent.ToString() == "Google")
            {
                return "Google";
            }
            else if (userAgent.Contains("facebookexternalhit"))
            {
                return "Facebook";
            }
            else
            {
                return "Unknown";
            }
        }

    }
}
