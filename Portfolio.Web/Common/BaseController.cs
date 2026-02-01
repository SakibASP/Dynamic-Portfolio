using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Portfolio.Models;
using System.Security.Claims;
using Portfolio.Repositories.Data;
using Serilog;

namespace Portfolio.Web.Common;

public class BaseController : Controller 
{
    private static readonly TimeZoneInfo bdTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Bangladesh Standard Time");
    public string? CurrentUserId { get; set; }
    public string? CurrentUserName { get; set; }
    public static DateTime BdCurrentTime { get; set; } = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, bdTimeZone);

    public override async Task OnActionExecutionAsync(ActionExecutingContext filterContext, ActionExecutionDelegate next)
    {
        using PortfolioDbContext _context = new();

        if (filterContext.HttpContext.User.Identity!.IsAuthenticated)
        {
            CurrentUserName = filterContext.HttpContext.User.Identity.Name;
            CurrentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        try
        {
            var ipAddress = filterContext.HttpContext.Connection.RemoteIpAddress?.ToString();
            var userAgent = filterContext.HttpContext.Request.Headers.UserAgent.ToString();
            var userInfo = Utility.GetVisitorsDeviceInfo(userAgent);

            if (userInfo is not null)
            {
                var IsExists = await _context.Visitors
                    .AsNoTracking()
                    .AnyAsync(v => v.IPAddress == ipAddress && v.VisitTime.Date == BdCurrentTime.Date && v.OperatingSystem == userInfo.OperatingSystem && v.UserAgent == userAgent);

                if (!IsExists)
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
                    await _context.SaveChangesAsync();
                }
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"I am from BaseController OnActionExecutionAsync...");
        }
        finally
        {
            // Call the next action in the pipeline
            await next();
        }
    }

    

}
