using Microsoft.Extensions.Configuration;

namespace MovieVault.Core.Configuration
{
    public static class TMDBConfiguration
    {
        private static readonly IConfiguration _configuration;

        static TMDBConfiguration()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }
        public static IConfiguration GetConfiguration() => _configuration;

        public static string GetTmdbApiKey() =>
            _configuration["TMDB:ApiKey"] ?? throw new InvalidOperationException("TMDB API Key is missing.");
    }
}