using Microsoft.Extensions.DependencyInjection;
using MovieVault.UI.Forms;
using MovieVault.UI.UserControls;
using Serilog;

namespace MovieVault.UI.Configuration
{
    public static class DependencyInjection
    {
        public static ServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            var loggerFactory = MovieVault.UI.Configuration.LoggingConfig.GetLoggerFactory();
            services.AddSingleton(loggerFactory);
            services.AddLogging();

            Data.Configuration.DependencyInjection.ConfigureDataServices(services);
            Core.Configuration.DependencyInjection.ConfigureCoreServices(services);

            services.AddTransient<MainForm>();
            services.AddTransient<RegisterForm>();

            services.AddTransient<LoginUserControl>();

            return services.BuildServiceProvider();
        }
    }
}
