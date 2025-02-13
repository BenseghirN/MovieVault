using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;

namespace MovieVault.Data.Utilities
{
    public class DBHelperConfiguration
    {
        private static readonly IConfiguration _configuration;
        private static readonly ILoggerFactory _loggerFactory;

        static DBHelperConfiguration()
        {
            var logPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MovieVault", "logs", "database.log");
            Directory.CreateDirectory(Path.GetDirectoryName(logPath)!);

            // Initiate Serilog
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(logPath, rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7) // Keep 7 days
                .CreateLogger();

            Log.Information("Initializing DBHelper...");

            // Chargement de la configuration depuis l'UI (appsettings.json)
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // UI est le projet exécuté
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            _loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddSerilog();
            });
        }

        public static IConfiguration GetConfiguration() => _configuration;

        public static ILogger<T> GetLogger<T>() => _loggerFactory.CreateLogger<T>();

        public static string GetConnectionString(string name = "MovieVaultDB") =>
            _configuration.GetConnectionString(name) ?? throw new InvalidOperationException("Database connection string is missing.");
    }
}
