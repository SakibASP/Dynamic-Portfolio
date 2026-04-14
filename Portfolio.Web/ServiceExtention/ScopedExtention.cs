namespace Portfolio.Web.ServiceExtention;

/// <summary>
/// Web-layer scoped registrations. Repositories and infrastructure services are
/// registered by <see cref="Portfolio.Infrastructure.InfrastructureServiceCollectionExtensions.AddInfrastructure"/>.
/// </summary>
public static class ScopedExtention
{
    public static IServiceCollection AddAllScoped(this IServiceCollection services)
    {
        return services;
    }
}
