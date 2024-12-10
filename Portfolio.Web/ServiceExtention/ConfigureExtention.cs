using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Portfolio.Repositories.Data;
using Portfolio.Utils;
using Portfolio.Web.Data;

namespace Portfolio.Web.ServiceExtention
{
    public static class ConfigureExtention
    {
        public static IServiceCollection AddConfigurations(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            //getting the connection string
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            connectionString = EncryptionHelper.Decrypt(connectionString);

            //adding dbContext for crud operations
            serviceCollection.AddDbContext<PortfolioDbContext>(options =>
                options.UseSqlServer(connectionString));

            //adding dbContext for identity purpose
            serviceCollection.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            serviceCollection.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            
            //setting up error page
            serviceCollection.AddDatabaseDeveloperPageExceptionFilter();

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
