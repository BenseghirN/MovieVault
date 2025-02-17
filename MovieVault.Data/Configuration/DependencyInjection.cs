using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MovieVault.Data.Interfaces;
using MovieVault.Data.Repositories;
using MovieVault.Data.Utilities;
using Serilog;

namespace MovieVault.Data.Configuration
{
    public static class DependencyInjection
    {
        public static void ConfigureDataServices(IServiceCollection services)
        {
            var configuration = DBHelperConfiguration.GetConfiguration();
            var loggerFactory = LoggerFactory.Create(builder => builder.AddSerilog());

            services.AddSingleton(configuration);
            services.AddSingleton(loggerFactory);
            services.AddSingleton<IDatabaseConnection>(sp => new DatabaseConnection(sp.GetRequiredService<ILogger<DatabaseConnection>>()));
            services.AddSingleton<IDBHelper, DBHelper>();
            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddSingleton<IMovieRepository, MovieRepository>();
            services.AddLogging();
        }
    }
}
