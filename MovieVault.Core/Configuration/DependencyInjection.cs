using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MovieVault.Core.Interfaces;
using MovieVault.Core.Services;
using MovieVault.Data.Interfaces;

namespace MovieVault.Core.Configuration
{
    public static class DependencyInjection
    {
        public static void ConfigureCoreServices(IServiceCollection services)
        {
            var loggerFactory = LoggingConfig.GetLoggerFactory();

            services.AddSingleton(loggerFactory);
            services.AddSingleton<IUserService>(sp => new UserService(sp.GetRequiredService<IUserRepository>(), sp.GetRequiredService<ILogger<UserService>>()));
            services.AddSingleton<IUserService, UserService>();
            services.AddLogging();
        }
    }
}
