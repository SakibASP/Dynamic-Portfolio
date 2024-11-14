using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using SAKIB_PORTFOLIO.Common;
using SAKIB_PORTFOLIO.Data;
using SAKIB_PORTFOLIO.Middlewares;
using UAParser;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();

//Session
builder.Services.AddSession(options =>
{
    options.Cookie.Name = Constant.portfolionSession;
    //options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
//builder.Services.AddDistributedMemoryCache();

//email setting
EmailSettings emailSettings = new();
builder.Services.AddTransient<IEmailSender>(provider =>
     new SendEmail(emailSettings));

builder.Services.AddMvc();

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
    string userAgent = context.Request.Headers.UserAgent.ToString();
    if (userAgent.Contains("Mediatoolkitbot"))
    {
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        await context.Response.WriteAsync("Access Forbidden for Mediatoolkitbot");
        return;
    }
    //this a facebook bot
    else if (userAgent.Contains("WhatsApp"))
    {
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        await context.Response.WriteAsync("Sakib has restricted WhatsApp bot/spider for the site. Please use an external browser.");
        return;
    }

    var uaParser = Parser.GetDefault();
    ClientInfo? clientInfo = uaParser.Parse(userAgent);
    if (clientInfo != null)
    {
        if (clientInfo is not null && clientInfo.UserAgent.Family.Contains("bot", StringComparison.CurrentCultureIgnoreCase) || clientInfo.Device.Family.Contains("spider", StringComparison.CurrentCultureIgnoreCase))
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync("Sakib has restricted Bots and spiders for the site.");
            return;
        }
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
