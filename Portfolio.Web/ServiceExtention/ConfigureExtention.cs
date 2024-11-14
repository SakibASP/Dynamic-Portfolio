using Microsoft.AspNetCore.Identity;
using Portfolio.Utils;
using Portfolio.Web.Data;

namespace Portfolio.Web.ServiceExtention
{
    public static class ConfigureExtention
    {
        public static IServiceCollection AddConfigurations(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            // Add services to the container.
            serviceCollection.AddControllersWithViews();
            serviceCollection.AddMvc();

            //Session
            serviceCollection.AddSession(options =>
            {
                options.Cookie.Name = Constant.portfolionSession;
                //options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            return serviceCollection;
        }
    }
}
