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

            services.AddSingleton<IGenreRepository, GenreRepository>();
            services.AddSingleton<IMovieRepository, MovieRepository>();
            services.AddSingleton<IMoviesGenresRepository, MoviesGenresRepository>();
            services.AddSingleton<IMoviesPeopleRepository, MoviePeopleRepository>();
            services.AddSingleton<IPeopleRepository, PeopleRepository>();
            services.AddSingleton<IReviewRepository, ReviewRepository>();
            services.AddSingleton<IUserMoviesRepository, UserMovieRepository>();
            services.AddSingleton<IUserRepository, UserRepository>();

            services.AddLogging();
        }
    }
}
