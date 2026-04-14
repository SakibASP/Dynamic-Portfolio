using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Portfolio.Application.Abstractions;
using Portfolio.Infrastructure.Adapters;
using Portfolio.Infrastructure.Persistence;
using Portfolio.Application.Common;

namespace Portfolio.Infrastructure;

/// <summary>
/// Composition root for the Infrastructure layer. The Web project calls this once
/// from Program.cs — it does not otherwise take a direct dependency on concrete
/// repositories or adapters.
/// </summary>
public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = EncryptionHelper.Decrypt(configuration.GetConnectionString("DefaultConnection"));

        // Single DbContext for domain + Identity.
        services.AddDbContext<PortfolioDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<PortfolioDbContext>();

        // Repositories (persistence abstractions).
        services.AddScoped<IProfileRepo, ProfileRepo>();
        services.AddScoped<IDescriptionTypeRepo, DescriptionTypeRepo>();
        services.AddScoped<IDescriptionRepo, DescriptionRepo>();
        services.AddScoped<IContactsRepo, ContactsRepo>();
        services.AddScoped<ISkillsRepo, SkillsRepo>();
        services.AddScoped<IEducationRepo, EducationRepo>();
        services.AddScoped<IExperienceRepo, ExperienceRepo>();
        services.AddScoped<IProjectRepo, ProjectRepo>();
        services.AddScoped<IProfileCoverRepo, ProfileCoverRepo>();
        services.AddScoped<IHomeRepo, HomeRepo>();

        // External adapters.
        services.AddHttpClient(GeoLocationClient.HttpClientName, c =>
        {
            c.Timeout = TimeSpan.FromSeconds(5);
        });
        services.AddScoped<IGeoLocationClient, GeoLocationClient>();
        services.AddScoped<IVisitorTrackingService, VisitorTrackingService>();
        services.AddTransient<IEmailSender, EmailSender>();

        return services;
    }
}
