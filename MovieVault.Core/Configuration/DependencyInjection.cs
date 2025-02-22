using Microsoft.Extensions.DependencyInjection;
using MovieVault.Core.Interfaces;
using MovieVault.Core.Services;
using MovieVault.Core.TMDB;

namespace MovieVault.Core.Configuration
{
    public static class DependencyInjection
    {
        public static void ConfigureCoreServices(IServiceCollection services)
        {
            var configuration = TMDBConfiguration.GetConfiguration();
            services.AddSingleton(configuration);

            var loggerFactory = MovieVault.Core.Configuration.LoggingConfig.GetLoggerFactory();
            services.AddSingleton(loggerFactory);
            services.AddLogging();

            services.AddSingleton<IGenreService, GenreService>();
            services.AddSingleton<IMovieManagerService, InsertNewMovieService>();
            services.AddSingleton<IMovieService, MovieService>();
            services.AddSingleton<IMoviesGenresService, MoviesGenresService>();
            services.AddSingleton<IMoviesPeopleService, MoviesPeopleService>();
            services.AddSingleton<IPeopleService, PeopleService>();
            services.AddSingleton<IReviewService, ReviewService>();
            services.AddSingleton<IUserMoviesService, UserMovieService>();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<ITmdbService, TmdbService>();
            services.AddSingleton<IMovieDetailsService, MovieDetailsService>();

        }
    }
}
