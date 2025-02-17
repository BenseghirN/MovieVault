using Microsoft.Extensions.DependencyInjection;
using MovieVault.Core.Interfaces;
using MovieVault.Core.Services;

namespace MovieVault.Core.Configuration
{
    public static class DependencyInjection
    {
        public static void ConfigureCoreServices(IServiceCollection services)
        {
            var loggerFactory = LoggingConfig.GetLoggerFactory();

            services.AddSingleton(loggerFactory);
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IMovieService, MovieService>();
            services.AddSingleton<IMovieManagerService, MovieManagerService>();
            services.AddLogging();
        }
    }
}
