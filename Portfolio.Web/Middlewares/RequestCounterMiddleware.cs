using Microsoft.EntityFrameworkCore;
using Portfolio.Models;
using Portfolio.Repositories.Data;
using Portfolio.Utils;
using Serilog;

namespace Portfolio.Web.Middlewares
{
    public class RequestCounterMiddleware(RequestDelegate next)
    {
        // Specify the time zone for Bangladesh
        private static readonly TimeZoneInfo bdTimeZone = TimeZoneInfo.FindSystemTimeZoneById(Constant.bangladeshTimezone);

        private readonly RequestDelegate _next = next;

        public async Task Invoke(HttpContext context, PortfolioDbContext _context)
        {
            DateTime currentDate = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, bdTimeZone); // Get current date in UTC

            try
            {
                // Check if there is a record for the current date
                var requestCounts = await _context.RequestCounts.FirstOrDefaultAsync(rc => rc.LastUpdated.Date == currentDate.Date);

                if (requestCounts == null)
                {
                    // If no record exists, create a new one
                    requestCounts = new RequestCounts
                    {
                        LastUpdated = currentDate
                    };
                    await _context.RequestCounts.AddAsync(requestCounts);
                }
                else
                {
                    requestCounts.LastUpdated = currentDate;
                }

                // Increment counts based on request method
                if (context.Request.Method == "GET")
                {
                    requestCounts.GetCount++;
                }
                else if (context.Request.Method == "POST")
                {
                    requestCounts.PostCount++;
                }

                await _context.SaveChangesAsync();

                await _next(context);
            }
            catch(Exception ex) 
            {
                Log.Error(ex, $"I am from RequestCounterMiddleware Invoke...");
            }
        }
    }
}
