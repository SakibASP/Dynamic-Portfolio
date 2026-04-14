namespace Portfolio.Web.ServiceExtention;

/// <summary>
/// Web-layer transient registrations. IEmailSender is registered inside
/// <see cref="Portfolio.Infrastructure.InfrastructureServiceCollectionExtensions.AddInfrastructure"/>.
/// </summary>
public static class TransientExtention
{
    public static IServiceCollection AddAllTransient(this IServiceCollection services)
    {
        return services;
    }
}
