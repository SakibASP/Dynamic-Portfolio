using Microsoft.EntityFrameworkCore;
using SAKIB_PORTFOLIO.Data;
using SAKIB_PORTFOLIO.Models;

namespace SAKIB_PORTFOLIO.Middlewares
{
    public class RequestCounterMiddleware(RequestDelegate next)
    {
        // Specify the time zone for Bangladesh
        private static readonly TimeZoneInfo bdTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Bangladesh Standard Time");

        private readonly RequestDelegate _next = next;

        public async Task Invoke(HttpContext context, ApplicationDbContext _context)
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
            catch 
            {
                // Exception part
            }
        }
    }
}
