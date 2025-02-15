using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace MovieVault.UI.Configuration
{
    public static class DependencyInjection
    {
        public static ServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            services.AddSingleton(LoggingConfig.GetLoggerFactory());
            services.AddLogging(builder => builder.AddSerilog());

            Data.Configuration.DependencyInjection.ConfigureDataServices(services);
            Core.Configuration.DependencyInjection.ConfigureCoreServices(services);

            return services.BuildServiceProvider();
        }
    }
}
