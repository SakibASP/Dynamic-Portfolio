namespace Portfolio.Web.ServiceExtention
{
    public static class SingletonExtention
    {
        public static IServiceCollection AddAllSingleton(this IServiceCollection serviceCollection)
        {
            return serviceCollection;
        }
    }
}
