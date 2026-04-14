using Microsoft.Extensions.DependencyInjection;
using Portfolio.Application;
using Portfolio.Application.Common;
using Portfolio.Infrastructure;
using Portfolio.Web.Filters;

namespace Portfolio.Web.ServiceExtention;

/// <summary>
/// Presentation-layer composition root. Wires Application (use-case services) and
/// Infrastructure (EF + Identity + adapters) via their own AddApplication /
/// AddInfrastructure extensions, then layers on MVC-only concerns.
/// </summary>
public static class ConfigureExtention
{
    public static IServiceCollection AddConfigurations(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddInfrastructure(configuration);
        services.AddApplication();

        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddControllersWithViews(options =>
        {
            // Global action filter; replaces the 90-line god-method that used to sit
            // on BaseController. Delegates to IVisitorTrackingService.
            options.Filters.Add<VisitorTrackingFilter>();
        });
        services.AddRazorPages();

        services.AddSession(options =>
        {
            options.Cookie.Name = Constant.portfolionSession;
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });

        services.AddScoped<VisitorTrackingFilter>();
        return services;
    }
}
