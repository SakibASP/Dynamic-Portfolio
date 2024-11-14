using Microsoft.EntityFrameworkCore;
using Portfolio.Web.Data;
using Portfolio.Web.Middlewares;
using Portfolio.Web.ServiceExtention;
using Serilog.Events;
using Serilog;
using UAParser;

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
        TimeZoneInfo bdTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Bangladesh Standard Time");
        DateTime BdCurrentTime  = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, bdTimeZone);

        string userAgent = context.Request.Headers.UserAgent.ToString();
        if (userAgent.Contains("Mediatoolkitbot"))
        {
            Log.Information($"Visit by Mediatoolkitbot at {BdCurrentTime}");
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync("Access Forbidden for Mediatoolkitbot");
            return;
        }
        //this a facebook bot
        else if (userAgent.Contains("WhatsApp"))
        {
            Log.Information($"Visit From WhatsApp at {BdCurrentTime}");
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync("Sakib has restricted WhatsApp bot/spider for the site. Please use an external browser.");
            return;
        }

        var uaParser = Parser.GetDefault();
        ClientInfo? clientInfo = uaParser.Parse(userAgent);
        if (clientInfo != null)
        {
            if (clientInfo.UserAgent.Family.Contains("bot", StringComparison.CurrentCultureIgnoreCase))
            {
                Log.Information($"Visit by Bot at {BdCurrentTime}. Details : {clientInfo.UserAgent.Family}");
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Sakib has restricted Bots for the site.");
                return;
            }
            if(clientInfo.Device.Family.Contains("spider", StringComparison.CurrentCultureIgnoreCase))
            {
                Log.Information($"Visit by Spider at {BdCurrentTime}. Details : {clientInfo.Device.Family}");
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Sakib has restricted Spiders for the site.");
                return;
            }
        }

        // Restrict access to Identity/Register page
        if (context.Request.Path.StartsWithSegments("/Identity/Account/Register"))
        {
            var ipAddress = context.Connection.RemoteIpAddress?.ToString();
            Log.Information($"Access attempt to Register page at {BdCurrentTime}. IP : {ipAddress}");
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
