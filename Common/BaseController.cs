using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using SAKIB_PORTFOLIO.Data;
using SAKIB_PORTFOLIO.Models;
using System.Security.Claims;
using UAParser;

namespace SAKIB_PORTFOLIO.Common
{
    public class BaseController : Controller 
    {
        private static readonly TimeZoneInfo bdTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Bangladesh Standard Time");
        public string? CurrentUserId { get; set; }
        public string? CurrentUserName { get; set; }
        public static DateTime BdCurrentTime { get; set; } = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, bdTimeZone);

        //public List<MY_PROFILE>? S_MY_PROFILE { get; set; }
        //public List<PROFILE_COVER>? S_PROFILE_COVER { get; set; }
        //public List<PROJECTS>? S_PROJECTS { get; set; }
        //public List<EDUCATION>? S_EDUCATION { get; set; }
        //public List<EXPERIENCE>? S_EXPERIENCE { get; set; }
        //public List<CONTACTS>? S_CONTACTS { get; set; }
        //public List<MY_SKILLS>? S_MY_SKILLS { get; set; }

        private readonly ApplicationDbContext _context = new();

        public override async void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (filterContext.HttpContext.User.Identity!.IsAuthenticated)
            {
                CurrentUserName = filterContext.HttpContext.User.Identity.Name;
                CurrentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            }

            try
            {
                var request = filterContext.HttpContext.Request;
                if (request.Method == "GET" || request.Method == "POST")
                {
                    RequestCounts? requestCounts = await _context.RequestCounts.FirstOrDefaultAsync(x => x.LastUpdated.Date == BdCurrentTime.Date);

                    if (requestCounts is null)
                    {
                        requestCounts = new RequestCounts
                        {
                            LastUpdated = BdCurrentTime,
                        };
                        await _context.RequestCounts.AddAsync(requestCounts);
                    }
                    else
                    {
                        requestCounts.LastUpdated = BdCurrentTime;
                    }

                    if (request.Method == "POST")
                    {
                        requestCounts.PostCount++;
                    }

                    if (request.Method == "GET")
                    {
                        requestCounts.GetCount++;
                    }

                    var ipAddress = filterContext.HttpContext.Connection.RemoteIpAddress?.ToString(); //"202.5.58.3";
                    var userAgent = filterContext.HttpContext.Request.Headers.UserAgent.ToString(); // Extract browser info from User-Agent header
                    var userInfo = GetVisitorsDeviceInfo(userAgent); // Extract operating system from User-Agent header

                    if(userInfo is not null)
                    {
                        var existingVisitor = await _context.Visitors
                            .FirstOrDefaultAsync(v => v.IPAddress == ipAddress && v.VisitTime.Date == BdCurrentTime.Date && v.OperatingSystem == userInfo.OperatingSystem && v.UserAgent == userAgent);

                        if (existingVisitor is null)
                        {
                            Visitors visitor = GetVisitorsDeviceInfo(userAgent);
                            
                            //// Create a service collection
                            //var services = new ServiceCollection();
                            //// Add HttpClientFactory to the service collection
                            //services.AddHttpClient();
                            //// Build the service provider
                            //var serviceProvider = services.BuildServiceProvider();
                            //// Use the service provider to create an instance of HttpClientFactory
                            //var _httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();

                            // Make request to ip-api.com API
                            using var client = new HttpClient(); //_httpClientFactory.CreateClient();

                            Dictionary<string, dynamic> dictVisitor = [];

                            var response = await client.GetAsync($"http://ip-api.com/json/{ipAddress}?fields=city,country,zip,timezone,isp,org,as");
                            if (response.IsSuccessStatusCode)
                            {
                                // Parse JSON response
                                var content = await response.Content.ReadAsStringAsync();
                                dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(content)!;

                                if (result != null)
                                {
                                    // Extract location information
                                    dictVisitor.Add("City", result.city);
                                    dictVisitor.Add("Country", result.country);
                                    dictVisitor.Add("Zip", result.zip);
                                    dictVisitor.Add("Timezone", result.timezone);
                                    dictVisitor.Add("Isp", result.isp);
                                    dictVisitor.Add("Org", result.org);
                                    dictVisitor.Add("As", result["as"]);
                                }
                            }
                            else
                            {
                                // Handle error
                                //return StatusCode((int)response.StatusCode);
                            }

                            visitor.City = dictVisitor["City"];
                            visitor.Country = dictVisitor["Country"];
                            visitor.Zip = dictVisitor["Zip"];
                            visitor.Timezone = dictVisitor["Timezone"];
                            visitor.Isp = dictVisitor["Isp"];
                            visitor.Org = dictVisitor["Org"];
                            visitor.As = dictVisitor["As"];
                            visitor.IPAddress = ipAddress;
                            visitor.UserAgent = userAgent;
                            visitor.VisitTime = BdCurrentTime;

                            await _context.Visitors.AddAsync(visitor);
                        }

                        await _context.SaveChangesAsync();
                    }

                }
            }
            catch(Exception ex) 
            {
                Console.WriteLine(ex.ToString());
            }

           
            ////Populating the sessions
            //var sessionProfife = HttpContext.Session.GetObjectFromJsonList<MY_PROFILE>(Constant.myProfile);
            //if (sessionProfife != null)
            //{
            //    S_MY_PROFILE = (List<MY_PROFILE>)sessionProfife;
            //}
            //else
            //{
            //    S_MY_PROFILE = _context.MY_PROFILE.ToList();
            //    HttpContext.Session.SetObjectAsJson<MY_PROFILE>(Constant.myProfile, S_MY_PROFILE);
            //}

            //var sessionProfifeCover = HttpContext.Session.GetObjectFromJsonList<PROFILE_COVER>(Constant.myProfileCover);
            //if (sessionProfifeCover != null)
            //{
            //    S_PROFILE_COVER = (List<PROFILE_COVER>)sessionProfifeCover;
            //}
            //else
            //{
            //    S_PROFILE_COVER = _context.PROFILE_COVER.ToList();
            //    HttpContext.Session.SetObjectAsJson<MY_PROFILE>(Constant.myProfileCover, S_PROFILE_COVER);
            //}

            //var sessionProject = HttpContext.Session.GetObjectFromJsonList<PROJECTS>(Constant.myProject);
            //if (sessionProject != null)
            //{
            //    S_PROJECTS = (List<PROJECTS>)sessionProject;
            //}
            //else
            //{
            //    S_PROJECTS = _context.PROJECTS.ToList();
            //    HttpContext.Session.SetObjectAsJson<MY_PROFILE>(Constant.myProject, S_PROJECTS);
            //}

            //var sessionEducation = HttpContext.Session.GetObjectFromJsonList<EDUCATION>(Constant.myEducation);
            //if (sessionEducation != null)
            //{
            //    S_EDUCATION = (List<EDUCATION>)sessionEducation;
            //}
            //else
            //{
            //    S_EDUCATION = _context.EDUCATION.ToList();
            //    HttpContext.Session.SetObjectAsJson<MY_PROFILE>(Constant.myEducation, S_EDUCATION);
            //}

            //var sessionExperience = HttpContext.Session.GetObjectFromJsonList<EXPERIENCE>(Constant.myExperience);
            //if (sessionExperience != null)
            //{
            //    S_EXPERIENCE = (List<EXPERIENCE>)sessionExperience;
            //}
            //else
            //{
            //    S_EXPERIENCE = _context.EXPERIENCE.ToList();
            //    HttpContext.Session.SetObjectAsJson<MY_PROFILE>(Constant.myExperience, S_EXPERIENCE);
            //}

            //var sessionSkills = HttpContext.Session.GetObjectFromJsonList<MY_SKILLS>(Constant.mySkill);
            //if (sessionSkills != null)
            //{
            //    S_MY_SKILLS = (List<MY_SKILLS>)sessionSkills;
            //}
            //else
            //{
            //    S_MY_SKILLS = _context.MY_SKILLS.ToList();
            //    HttpContext.Session.SetObjectAsJson<MY_SKILLS>(Constant.mySkill, S_MY_SKILLS);
            //}

            //var sessionContacts = HttpContext.Session.GetObjectFromJsonList<CONTACTS>(Constant.myContact);
            //if (sessionContacts != null)
            //{
            //    S_CONTACTS = (List<CONTACTS>)sessionContacts;
            //}
            //else
            //{
            //    S_CONTACTS = _context.CONTACTS.ToList();
            //    HttpContext.Session.SetObjectAsJson<MY_SKILLS>(Constant.myContact, S_CONTACTS);
            //}
        }

        private static Visitors GetVisitorsDeviceInfo(string userAgent)
        {
            var uaParser = Parser.GetDefault();
            ClientInfo clientInfo = uaParser.Parse(userAgent);

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
