﻿using Microsoft.Extensions.DependencyInjection;
using MovieVault.Core.Interfaces;
using MovieVault.Core.Services;

namespace MovieVault.Core.Configuration
{
    public static class DependencyInjection
    {
        public static void ConfigureCoreServices(IServiceCollection services)
        {
            var configuration = TMDBConfiguration.GetConfiguration();
            var loggerFactory = LoggingConfig.GetLoggerFactory();

            services.AddSingleton(configuration);
            services.AddSingleton(loggerFactory);

            services.AddSingleton<IGenreService, GenreService>();
            services.AddSingleton<IMovieManagerService, InsertNewMovieService>();
            services.AddSingleton<IMovieService, MovieService>();
            services.AddSingleton<IMoviesGenresService, MoviesGenresService>();
            services.AddSingleton<IMoviesPeopleService, MoviesPeopleService>();
            services.AddSingleton<IPeopleService, PeopleService>();
            services.AddSingleton<IReviewService, ReviewService>();
            services.AddSingleton<IUserMoviesService, UserMovieService>();
            services.AddSingleton<IUserService, UserService>();

            services.AddLogging();
        }
    }
}
