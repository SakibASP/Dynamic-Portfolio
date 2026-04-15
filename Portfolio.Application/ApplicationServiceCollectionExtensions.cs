using Microsoft.Extensions.DependencyInjection;
using Portfolio.Application.Services;

namespace Portfolio.Application;

/// <summary>
/// Composition-root extension for the Application layer. Registers use-case services.
/// Persistence (IxxxRepo) and external adapters (IEmailSender, IGeoLocationClient,
/// IVisitorTrackingService) are registered by <c>AddInfrastructure</c> in the
/// Infrastructure layer.
/// </summary>
public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IProfileService, ProfileService>();
        services.AddScoped<IContactsService, ContactsService>();
        services.AddScoped<IDescriptionService, DescriptionService>();
        services.AddScoped<IDescriptionTypeService, DescriptionTypeService>();
        services.AddScoped<IEducationService, EducationService>();
        services.AddScoped<IExperienceService, ExperienceService>();
        services.AddScoped<ISkillsService, SkillsService>();
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<IProfileCoverService, ProfileCoverService>();
        services.AddScoped<IHomeService, HomeService>();
        services.AddScoped<IBlogService, BlogService>();
        services.AddScoped<IEmailSenderRelay, EmailSenderRelay>();
        return services;
    }
}
