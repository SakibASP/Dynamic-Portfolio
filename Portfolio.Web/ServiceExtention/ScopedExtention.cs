using Portfolio.Interfaces;
using Portfolio.Web.Common;
using Portfolio.Web.Repository;

namespace Portfolio.Web.ServiceExtention
{
    public static class ScopedExtention
    {
        public static IServiceCollection AddAllScoped(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<SendEmail>();
            serviceCollection.AddScoped<IProfileRepo, ProfileRepo>();
            serviceCollection.AddScoped<IDescriptionTypeRepo, DescriptionTypeRepo>();
            serviceCollection.AddScoped<IDescriptionRepo, DescriptionRepo>();

            return serviceCollection;
        }
    }
}
