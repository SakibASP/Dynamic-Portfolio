using Microsoft.AspNetCore.Identity.UI.Services;
using Portfolio.Web.Common;

namespace Portfolio.Web.ServiceExtention
{
    public static class TransientExtention
    {
        public static IServiceCollection AddAllTransient(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IEmailSender, SendEmail>();

            return serviceCollection;
        }
    }
}
