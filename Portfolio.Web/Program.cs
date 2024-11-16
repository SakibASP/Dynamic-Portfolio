using Microsoft.EntityFrameworkCore;
using Portfolio.Web.Data;
using Portfolio.Web.Middlewares;
using Portfolio.Web.ServiceExtention;
using Serilog.Events;
using Serilog;
using UAParser;
using Portfolio.Utils;

//Creating Serilog configuration
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("System", LogEventLevel.Warning)
    .MinimumLevel.Override("AspNetCore", LogEventLevel.Warning)
    .WriteTo.Logger(lc => lc
        .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information)
        .WriteTo.File("Logs/InfoLogs/logs-.txt", rollingInterval: RollingInterval.Day)
    )
    .WriteTo.Logger(lc => lc
        .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Fatal)
        .WriteTo.File("Logs/FatalLogs/logs-.txt", rollingInterval: RollingInterval.Day)
    )
    .WriteTo.Logger(lc => lc
        .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error)
        .WriteTo.File("Logs/ErrorLogs/logs-.txt", rollingInterval: RollingInterval.Day)
    )
    .Filter.ByExcluding(e => e.Properties.ContainsKey("SourceContext") &&
                              (e.Properties["SourceContext"].ToString().Contains("Microsoft.EntityFrameworkCore")
                              || e.MessageTemplate.Text.Contains("HTTP")
    ))
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString));
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();

    //Registering different service lifetime
    builder.Services.AddConfigurations();
    builder.Services.AddAllTransient();
    builder.Services.AddAllScoped();
    builder.Services.AddAllSingleton();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseMigrationsEndPoint();
    }
    else
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    // Filter access from different bot browsers
    app.Use(async (context, next) =>
    {
        var bdTimeZone = TimeZoneInfo.FindSystemTimeZoneById(Constant.bangladeshTimezone);
        var BdCurrentTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, bdTimeZone);
        var ipAddress = context.Connection.RemoteIpAddress?.ToString();
        var userAgent = context.Request.Headers.UserAgent.ToString();
        var uaParser = Parser.GetDefault();
        var clientInfo = uaParser.Parse(userAgent);

        //Restricting different bot from visiting the site
        if (userAgent.Contains("Mediatoolkitbot"))
        {
            Log.Information($"Visit by Mediatoolkitbot at [{BdCurrentTime}] , ip [{ipAddress}] url [{context.Request.Path}]");
            //context.Response.StatusCode = StatusCodes.Status403Forbidden;
            //await context.Response.WriteAsync("Access Forbidden for Mediatoolkitbot");
            //return;
        }
        //this a facebook bot
        else if (userAgent.Contains("WhatsApp"))
        {
            Log.Information($"Visit From WhatsApp at [{BdCurrentTime}], ip [{ipAddress}] url [{context.Request.Path}]");
            //context.Response.StatusCode = StatusCodes.Status403Forbidden;
            //await context.Response.WriteAsync("Sakib has restricted WhatsApp bot/spider for the site. Please use an external browser.");
            //return;
        }
        if (clientInfo != null)
        {
            if (clientInfo.UserAgent.Family.Contains("bot", StringComparison.CurrentCultureIgnoreCase))
            {
                Log.Information($"Visit by Bot at [{BdCurrentTime}]. name [{clientInfo.UserAgent.Family}], ip [{ipAddress}] url [{context.Request.Path}]");
                //context.Response.StatusCode = StatusCodes.Status403Forbidden;
                //await context.Response.WriteAsync("Sakib has restricted Bots for the site.");
                //return;
            }
            else if (clientInfo.Device.Family.Contains("spider", StringComparison.CurrentCultureIgnoreCase))
            {
                Log.Information($"Visit by Spider at [{BdCurrentTime}]. name [{clientInfo.Device.Family}] ip [{ipAddress}] url [{context.Request.Path}]");
                //context.Response.StatusCode = StatusCodes.Status403Forbidden;
                //await context.Response.WriteAsync("Sakib has restricted Spiders for the site.");
                //return;
            }
        }

        // Restrict access to Identity/Register page
        if (context.Request.Path.StartsWithSegments("/Identity/Account/Register"))
        {
            Log.Information($"Access attempt to Register page at [{BdCurrentTime}] ip [{ipAddress}]");
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync("Access to registration is denied.");
            return;
        }
        await next();
    });

    app.UseMiddleware<RequestCounterMiddleware>();

    //Session should be used before mvc
    app.UseSession();

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    app.MapRazorPages();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    await Log.CloseAndFlushAsync();
}
