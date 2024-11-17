using Portfolio.Interfaces;
using Portfolio.Repositories;
using Portfolio.Web.Common;

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
            serviceCollection.AddScoped<ISkillsRepo, SkillsRepo>();
            serviceCollection.AddScoped<IEducationRepo, EducationRepo>();

            return serviceCollection;
        }
    }
}
